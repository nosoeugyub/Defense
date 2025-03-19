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

    public bool CanMythUnit(MythUnitSO unitsodata)//�ش� ��ȭ���� �������� ���������� �ʵ忡�մ��� ����
    {
        Dictionary<UnitSO, int> fieldUnitCounts = new Dictionary<UnitSO, int>();

        // �ʵ忡 �ִ� ���ֵ��� ���� ����
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

        // ���տ� �ʿ��� ������ ��� �ʵ忡 �ִ��� Ȯ��
        foreach (var requiredUnit in unitsodata.Mix_Units)
        {
            if (!fieldUnitCounts.ContainsKey(requiredUnit))
            {
                return false; // �ش� ������ �ʵ忡 ����
            }

            // �ʿ��� �������� �ʵ忡 �ִ� ������ ������ false
            int requiredCount = 1; // ���տ� �ʿ��� ����
            if (fieldUnitCounts[requiredUnit] < requiredCount)
            {
                return false; // ���� ������ ����
            }
        }

        return true; // ��� ������ ����� ����
    }


    public Unit RandomFindUnit()//�������� ������ ã�� ��ȯ...
    {
        Unit temp_unit = new Unit();
        //���� Ȯ���� �ش� ��� ĳ���� ��ȯ  Ȯ���� �븻 95%  ���� 4% ���� 0.7% ��ȭ 0.3%
        UnitSO unitdata = DataManager.instance.GetRandomUnitData();
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname,transform.position, Quaternion.Euler(90,0,0)).GetComponent<Unit>();
        //temp_unit.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        return temp_unit;
    }

    public Unit RandomFindUnit(Utill_Enum.Unit_Grade grade)//��޿� �´� ���� ��������  ã�� ��ȯ...
    {
        Unit temp_unit = new Unit();
        //���� Ȯ���� �ش� ��� ĳ���� ��ȯ  Ȯ���� �븻 95%  ���� 4% ���� 0.7% ��ȭ 0.3%
        UnitSO unitdata = DataManager.instance.GetRandomUnitData(grade);
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<Unit>();
        //temp_unit.gameObject.transform.localScale = new Vector3(0.3f, 0.3f, 0);
        return temp_unit;
    }

    public Unit FindUnit(UnitSO unitdata)//�����Ϳ��´� ����ã��
    {
        Unit temp_unit = new Unit();
        string unitname = unitdata.UnitName;
        temp_unit = ObjectPooler.SpawnFromPool(unitname, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<Unit>();
        return temp_unit;
    }
    public void SummonUnit(bool ai)
    {
        if (CanSummon() && Canpeople())//��� üũ / �α��� üũ
        {
            Unit unit = RandomFindUnit();//�������� ��ȯ

            unit.Ai = ai; //�ش������� ai�� ��ȯ�޴��� ������ ��ȯ�ߴ��� �˱����ؼ�
            if (_Field.TryPlaceUnit(unit))
            {
                //��� ����
                DataManager.instance.UseGold(userdata, 20);
                //�α��� ����
                DataManager.instance.AddPeople(userdata, 1);
                //��ȯ ��� ����
                DataManager.instance.AddSpawnGold(userdata);

                //������ ���������� Ȯ��
                CanMakeMythUnit(ai);


                if (unit.Ai == false) //�÷��̾��� ui������Ʈ
                {
                    CurrencySystem.instance.UpdateSpawnGold(userdata);
                    CurrencySystem.instance.UpdateGold(userdata);
                    CurrencySystem.instance.UpdatePopulationText(userdata);
                }
                else //ai��� ��ȯ ���� �ռ��� ������ �������� ���ο� ���� �ռ��� ���� ���� �ߵ�
                {
                    //���� ����
                    for (int i = 0; i < DataManager.instance.MythUnitData.Length ; i++)
                    {
                        MythUnitSO mythsodata = DataManager.instance.MythUnitData[i];
                        if (mythsodata != null)
                        {
                            bool isCanMakeMyth = CanMythUnit(mythsodata);
                            if (isCanMakeMyth) //������ �����ϴٸ� 
                            {
                                MythUnitSpawnField(mythsodata , ai);
                            }
                        }
                    }
                    //2.�ռ� ����
                    CanCombinAi(unit);


                }
                Debug.Log($"{gameObject.name}��(��) ������ ��ȯ�߽��ϴ�! {unit.name} ���� ��: {Userdata.Gold}");
            }
            else
            {
                return; // ���� ���� �� ��ȯ ���
            }
        }
    }//��ȯ��ư���� ��ȯ...


    public void SummonUnit(bool ai, UnitSO unitso)
    {
        if (Canpeople())// �α��� üũ
        {
            Unit unit = FindUnit(unitso);//�������� ��ȯ

            unit.Ai = ai; //�ش������� ai�� ��ȯ�޴��� ������ ��ȯ�ߴ��� �˱����ؼ�
            if (_Field.TryPlaceUnit(unit))
            {
                //��� ����
                DataManager.instance.UseGold(userdata, 20);
                //�α��� ����
                DataManager.instance.AddPeople(userdata, 1);
                //��ȯ ��� ����
                DataManager.instance.AddSpawnGold(userdata);

                //������ ���������� Ȯ��
                CanMakeMythUnit(ai);


                if (unit.Ai == false) //�÷��̾��� ui������Ʈ
                {
                    CurrencySystem.instance.UpdateSpawnGold(userdata);
                    CurrencySystem.instance.UpdateGold(userdata);
                    CurrencySystem.instance.UpdatePopulationText(userdata);
                }
                else //ai��� ��ȯ ���� �ռ��� ������ �������� ���ο� ���� �ռ��� ���� ���� �ߵ�
                {
                    //����
                    for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
                    {
                        MythUnitSO mythsodata = DataManager.instance.MythUnitData[i];
                        if (mythsodata != null)
                        {
                            bool isCanMakeMyth = CanMythUnit(mythsodata);
                            if (isCanMakeMyth) //������ �����ϴٸ� 
                            {
                                MythUnitSpawnField(mythsodata, ai);
                                return;
                            }
                        }
                    }

                    //1.�ռ�
                    CanCombinAi(unit);



                }
                Debug.Log($"{gameObject.name}��(��) ������ ��ȯ�߽��ϴ�! {unit.name} ���� ��: {Userdata.Gold}");
            }
            else
            {
                return; // ���� ���� �� ��ȯ ���
            }
        }
    }//��ȯ��ư���� ��ȯ...



    public void SpawnMythUnit(UnitSO data  ,bool ai)
    {
        Unit unit = FindUnit(data);//�������� ��ȯ
        unit.Ai = ai; //�ش������� ai�� ��ȯ�޴��� ������ ��ȯ�ߴ��� �˱����ؼ�
        if (_Field.TryPlaceUnit(unit))
        {
            //��ȭ��� ��ȯ���� ����
            //�α� ui�� ������Ʈ
            DataManager.instance.AddPeople(Userdata, 1);

            if (!ai)//ai���  ui������Ʈ�¾��ص���
            {
                CurrencySystem.instance.UpdatePopulationText(Userdata);
            }
            
        }
    }


    //�Ǹ�
    public void SellUnit(FieldSlot slot)
    {
        int count = slot.UnitList.Count;
        Unit unit = slot.UnitList[0];
        int Gold = count * unit.UnitSo.SellCount;

        //�������� ��� ����
        DataManager.instance.AddGold(Userdata , Gold);
        //�α��� ���� 
        DataManager.instance.UsePeople(Userdata, count);
        //����Ʈ ����� ī��Ʈ�� 0 ���� �����Ը����
        slot.Init_Slot();
        //���� �������� Ȯ��
        CanMakeMythUnit(unit.Ai);
    }


    public void CanMakeMythUnit(bool _AI)
    {
        //��ȯ����������  ���յǴ��� Ȯ��
        List<MythUnitSO> sodata = new List<MythUnitSO>();
        for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
        {
            MythUnitSO tempdata = DataManager.instance.MythUnitData[i];
            bool isCanMythunit = CanMythUnit(tempdata); // ��ȯ�� ������ �ʵ忡�� �������� �˻�
            if (isCanMythunit)//���� ������ �����ϴٸ� 
            {
                sodata.Add(tempdata);

            }
        }
        //���� �̺�Ʈ ���� // ��ȭ ��ư�� ������ / ��ø���� �� / ��ư�� ���� ǥ��
        if (sodata.Count > 0 && sodata != null && _AI == false)
        {
            GameEventSystem.GameSynthesisUnitEvent(sodata);
        }
        else if(_AI == false)//�÷��̾ ��ȯ��������...
        {
            GameEventSystem.GameSynthesisUnitEvent(null);
        }
    }

    public void CanCombinAi(Unit unit)//Ai ,�ڵ� �ռ�
    {
        bool isAi = true;
        if (unit.fieldSlot.UnitList.Count == 3)//�ռ� ����
        {
            //�ռ� ����
            UnitCombine(unit.fieldSlot.UnitList, unit.fieldSlot , isAi);
        }
        else
        {
            return;
        }
    }

    public void UnitCombine(List<Unit> unit, FieldSlot slot , bool isAi)//���� �ռ�
    {
        //�α��� ���� 3
        DataManager.instance.UsePeople(Userdata, 3);

        //���� ������ ��� Ȯ��
        Utill_Enum.Unit_Grade grade = Utill_Enum.Unit_Grade.Normal;
        Unit tempunit = new Unit();
        //������� ���� ���� ������ ��������
        grade = Utill_Standard.UnitNextGrade(unit[0].UnitSo.UnitGrade);
        tempunit = RandomFindUnit(grade);
        tempunit.Ai = isAi;
        //�ش� tempui�� �̹� �ʵ忡 �ִ��� Ȯ��

        //�ű⼭ ��ȯ������ �ռ� ���� ����

        //�� �̻� �ռ��� ���Ѵٸ� ���� ��

        //���� ���� ����
        slot.Init_Slot();
        //�ش� �����ڸ��� �ش� ���� ��ȯ or ���� ���ֿ� ���ٿ� ����
        if (_Field.TryPlaceUnit(tempunit))
        {
            DataManager.instance.AddPeople(Userdata, 1);
            //��ȯ����������   �ش� ��ȯ
            

            //���յǴ��� Ȯ��
            CanMakeMythUnit(isAi);


        }
        if (isAi == false)//�÷��̾ �ռ��ҋ���..
        {
            //ui�� ���ֱ�
            UIPooling.Instance.HideAllButtons();
            //�α� ui�� ������Ʈ
            CurrencySystem.instance.UpdatePopulationText(Userdata);
        }
        
    }

    public void MythUnitSpawnField(MythUnitSO mythsodata , bool ai)//�������ֻ���
    {
        //�����Ѱ����� ����....
        //1.mythsodata�� ���տ� �ʿ��� ���ֵ� �� �����
        int mixcount = mythsodata.Mix_Units.Length;
        for (int i = 0; i < mixcount; i++)
        {
            _Field.RemoveCharactor(mythsodata.Mix_Units[i]);
        }
        //2.�α����� 
        DataManager.instance.UsePeople(Userdata, mixcount);
        //3.��ĭã�� //4.��ĭ�� ĳ���� ��ȯ
        SpawnMythUnit(mythsodata, ai); //��ȭ��޼�ȯ 
        //�׸��� �ٽ� ����ã��..
        CanMakeMythUnit(ai);
    }
}
