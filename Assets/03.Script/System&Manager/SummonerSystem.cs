using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SummonerSystem : MonoBehaviour
{
    [SerializeField] private Field _field;
    public Field _Field
    {
        get
        {
            return _field;
        }

        set
        {
            _field = value;
        }
    }

    [SerializeField] private UserSOData userdata;
    public UserSOData Userdata
    {
        get { return userdata; }
        set { userdata = value; }
    }



    public bool CanSummon()
    {
        return Userdata.Gold >= Userdata.UseGold;
    }

    public bool Canpeople()
    {
        return userdata.Currentpopulation < userdata.Maxpopulation; 
    }

    public bool CanMythUnit(MythUnitSO unitsodata)//해당 신화유닛 데이터의 조합유닛이 필드에잇는지 검토
    {
        Dictionary<UnitSO, int> fieldUnitCounts = new Dictionary<UnitSO, int>();

        // 필드에 있는 유닛들의 개수 세기
        foreach (var slot in _Field.FieldSlotgrid)
        {
            if (slot != null)
            {
                foreach (var placedUnit in slot.UnitList)
                {
                    if (fieldUnitCounts.ContainsKey(placedUnit.UnitSo))
                    {
                        fieldUnitCounts[placedUnit.UnitSo]++;
                    }
                    else
                    {
                        fieldUnitCounts[placedUnit.UnitSo] = 1;
                    }
                }
            }
        }

        // 조합에 필요한 유닛이 모두 필드에 있는지 확인
        foreach (var requiredUnit in unitsodata.Mix_Units)
        {
            if (!fieldUnitCounts.ContainsKey(requiredUnit))
            {
                return false; // 해당 유닛이 필드에 없음
            }

            // 필요한 개수보다 필드에 있는 개수가 적으면 false
            int requiredCount = 1; // 조합에 필요한 개수
            if (fieldUnitCounts[requiredUnit] < requiredCount)
            {
                return false; // 유닛 개수가 부족
            }
        }

        return true; // 모든 유닛이 충분히 있음
    }


    public Unit RandomFindUnit()//랜덤으로 유닛을 찾아 반환...
    {
        Unit temp_unit = new Unit();
        //랜덤 확률로 해당 등급 캐릭터 반환  확률을 노말 95%  레어 4% 영웅 0.7% 신화 0.3%
        UnitSO unitdata = DataManager.instance.GetRandomUnitData();
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname,transform.position, Quaternion.Euler(90,0,0)).GetComponent<Unit>();
        //temp_unit.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        return temp_unit;
    }

    public Unit RandomFindUnit(Utill_Enum.Unit_Grade grade)//등급에 맞는 유닛 랜덤으로  찾아 반환...
    {
        Unit temp_unit = new Unit();
        //랜덤 확률로 해당 등급 캐릭터 반환  확률을 노말 95%  레어 4% 영웅 0.7% 신화 0.3%
        UnitSO unitdata = DataManager.instance.GetRandomUnitData(grade);
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<Unit>();
        //temp_unit.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        return temp_unit;
    }

    public Unit FindUnit(UnitSO unitdata)//데이터에맞는 유닛찾기
    {
        Unit temp_unit = new Unit();
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<Unit>();
        return temp_unit;
    }
    public void SummonUnit(bool ai)
    {
        if (CanSummon() && Canpeople())//골드 체크 / 인구수 체크
        {
            Unit unit = RandomFindUnit();//랜덤유닛 소환

            unit.Ai = ai; //해당유닛이 ai가 소환햇는지 유저가 소환했는지 알기위해서
            if (_Field.TryPlaceUnit(unit))
            {
                //골드 감소
                DataManager.instance.UseGold(userdata, 20);
                //인구수 증가
                DataManager.instance.AddPeople(userdata, 1);
                //소환 골드 증가
                DataManager.instance.AddSpawnGold(userdata);

                //조합이 가능한지도 확인
                CanMakeMythUnit(ai);


                if (unit.Ai == false) //플레이어라면 ui업데이트
                {
                    CurrencySystem.instance.UpdateSpawnGold(userdata);
                    CurrencySystem.instance.UpdateGold(userdata);
                    CurrencySystem.instance.UpdatePopulationText(userdata);
                }
                else //ai라면 소환 직후 합성과 조합이 가능한지 여부에 따라 합성과 조합 로직 발동
                {
                    //조합 로직
                    for (int i = 0; i < DataManager.instance.MythUnitData.Length ; i++)
                    {
                        MythUnitSO mythsodata = DataManager.instance.MythUnitData[i];
                        if (mythsodata != null)
                        {
                            bool isCanMakeMyth = CanMythUnit(mythsodata);
                            if (isCanMakeMyth) //조합이 가능하다면 
                            {
                                MythUnitSpawnField(mythsodata , ai);
                            }
                        }
                    }
                    //2.합성 로직
                    CanCombinAi(unit);


                }
                Debug.Log($"{gameObject.name}이(가) 유닛을 소환했습니다! {unit.name} 남은 돈: {Userdata.Gold}");
            }
            else
            {
                return; // 공간 부족 시 소환 취소
            }
        }
    }//소환버튼으로 소환...


    public void SummonUnit(bool ai, UnitSO unitso)
    {
        if (Canpeople())// 인구수 체크
        {
            Unit unit = FindUnit(unitso);//랜덤유닛 소환

            unit.Ai = ai; //해당유닛이 ai가 소환햇는지 유저가 소환했는지 알기위해서
            if (_Field.TryPlaceUnit(unit))
            {
                //골드 감소
                DataManager.instance.UseGold(userdata, 20);
                //인구수 증가
                DataManager.instance.AddPeople(userdata, 1);
                //소환 골드 증가
                DataManager.instance.AddSpawnGold(userdata);

                //조합이 가능한지도 확인
                CanMakeMythUnit(ai);


                if (unit.Ai == false) //플레이어라면 ui업데이트
                {
                    CurrencySystem.instance.UpdateSpawnGold(userdata);
                    CurrencySystem.instance.UpdateGold(userdata);
                    CurrencySystem.instance.UpdatePopulationText(userdata);
                }
                else //ai라면 소환 직후 합성과 조합이 가능한지 여부에 따라 합성과 조합 로직 발동
                {
                    //조합
                    for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
                    {
                        MythUnitSO mythsodata = DataManager.instance.MythUnitData[i];
                        if (mythsodata != null)
                        {
                            bool isCanMakeMyth = CanMythUnit(mythsodata);
                            if (isCanMakeMyth) //조합이 가능하다면 
                            {
                                MythUnitSpawnField(mythsodata, ai);
                                return;
                            }
                        }
                    }

                    //1.합성
                    CanCombinAi(unit);



                }
                Debug.Log($"{gameObject.name}이(가) 유닛을 소환했습니다! {unit.name} 남은 돈: {Userdata.Gold}");
            }
            else
            {
                return; // 공간 부족 시 소환 취소
            }
        }
    }//소환버튼으로 소환...



    public void SpawnMythUnit(UnitSO data  ,bool ai)
    {
        Unit unit = FindUnit(data);//랜덤유닛 소환
        unit.Ai = ai; //해당유닛이 ai가 소환햇는지 유저가 소환했는지 알기위해서
        if (_Field.TryPlaceUnit(unit))
        {
            //신화등급 소환성공 로직
            //인구 ui도 없데이트
            DataManager.instance.AddPeople(Userdata, 1);

            if (!ai)//ai라면  ui업데이트는안해도됨
            {
                CurrencySystem.instance.UpdatePopulationText(Userdata);
            }
            
        }
    }


    //판매
    public void SellUnit(FieldSlot slot)
    {
        int count = slot.UnitList.Count;
        Unit unit = slot.UnitList[0];
        int Gold = count * unit.UnitSo.SellCount;

        //유저에게 골드 지급
        DataManager.instance.AddGold(Userdata , Gold);
        //인구수 감소 
        DataManager.instance.UsePeople(Userdata, count);
        //리스트 지우기 카운트도 0 만들어서 새슬롯만들기
        slot.Init_Slot();
        //조합 가능한지 확인
        CanMakeMythUnit(unit.Ai);
    }


    public void CanMakeMythUnit(bool _AI)
    {
        //소환성공했으니  조합되는지 확인
        List<MythUnitSO> sodata = new List<MythUnitSO>();
        for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
        {
            MythUnitSO tempdata = DataManager.instance.MythUnitData[i];
            bool isCanMythunit = CanMythUnit(tempdata); // 소환이 됐으니 필드에서 조합유닛 검색
            if (isCanMythunit)//만약 조합이 가능하다면 
            {
                sodata.Add(tempdata);

            }
        }
        //조합 이벤트 띄우기 // 신화 버튼의 조합텝 / 즉시만들기 텝 / 버튼의 숫자 표기
        if (sodata.Count > 0 && sodata != null && _AI == false)
        {
            GameEventSystem.GameSynthesisUnitEvent(sodata);
        }
        else if(_AI == false)//플레이어가 소환했을떄만...
        {
            GameEventSystem.GameSynthesisUnitEvent(null);
        }
    }

    public void CanCombinAi(Unit unit)//Ai ,자동 합성
    {
        bool isAi = true;
        if (unit.fieldSlot.UnitList.Count == 3)//합성 가능
        {
            //합성 절차
            UnitCombine(unit.fieldSlot.UnitList, unit.fieldSlot , isAi);
        }
        else
        {
            return;
        }
    }

    public void UnitCombine(List<Unit> unit, FieldSlot slot , bool isAi)//유닛 합성
    {
        //인구수 감소 3
        DataManager.instance.UsePeople(Userdata, 3);

        //현재 유닛의 등급 확인
        Utill_Enum.Unit_Grade grade = Utill_Enum.Unit_Grade.Normal;
        Unit tempunit = new Unit();
        //다음등급 몬스터 랜덤 데이터 가져오기
        grade = Utill_Standard.UnitNextGrade(unit[0].UnitSo.UnitGrade);
        tempunit = RandomFindUnit(grade);
        tempunit.Ai = isAi;
        //해당 tempui이 이미 필드에 있는지 확인

        //거기서 소환됐을때 합성 가능 한지

        //더 이상 합성을 못한다면 조합 비교

        //기존 슬롯 비우기
        slot.Init_Slot();
        //해당 유닛자리에 해당 몬스터 소환 or 기존 유닛에 더붙여 생성
        if (_Field.TryPlaceUnit(tempunit))
        {
            DataManager.instance.AddPeople(Userdata, 1);
            //소환성공했으니   해당 소환
            

            //조합되는지 확인
            CanMakeMythUnit(isAi);


        }
        if (isAi == false)//플레이어가 합성할떄만..
        {
            //ui도 꺼주기
            UIPooling.Instance.HideAllButtons();
            //인구 ui도 없데이트
            CurrencySystem.instance.UpdatePopulationText(Userdata);
        }
        
    }

    public void MythUnitSpawnField(MythUnitSO mythsodata , bool ai)//조합유닛생성
    {
        //성공한것으로 간주....
        //1.mythsodata의 조합에 필요한 유닛들 다 지우기
        int mixcount = mythsodata.Mix_Units.Length;
        for (int i = 0; i < mixcount; i++)
        {
            _Field.RemoveCharactor(mythsodata.Mix_Units[i]);
        }
        //2.인구감소 
        DataManager.instance.UsePeople(Userdata, mixcount);
        //3.빈칸찾기 //4.빈칸에 캐릭터 소환
        SpawnMythUnit(mythsodata, ai); //신화등급소환 
        //그리고 다시 조합찾기..
        CanMakeMythUnit(ai);
    }
}
