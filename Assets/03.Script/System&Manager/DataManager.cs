using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public UnitSO[] NomarlUnitData; //노말 유닛
    public UnitSO[] RareUnitData; // 레어 유닛
    public UnitSO[] HeroUnitData;//희귀 유닛
    public MythUnitSO[] MythUnitData;//신화 유닛
    public BossEnemySO[] BossSo; //보스테이터;
   
    //보스이름으로 보스 이미지 가져오기
    public Sprite GetBossSprite(string bossname)
    {
        Sprite tempsprite = null;

        for (int i = 0; i < BossSo.Length; i++)
        {
            if (BossSo[i].enemyName == bossname) 
            {
                tempsprite = BossSo[i].BossMainImg;
            }
        }

        return tempsprite;
    }


    //유저가 다이아 소모할떄
    public bool CanUseDia(UserSOData Userdata ,   int count)
    {
        int temp = Userdata.Dia - count;
        if (temp < 0)
        {
            return false; // 사용불가
        }
        else
        {
            return true;
        }
    }
    public void UseDia(UserSOData Userdata, int count)
    {
        bool usedia = CanUseDia(Userdata ,count);
        if (usedia)
        {
            Userdata.Dia -= count;
        }
    }

    //다이아 흭득할떄
    public void AddDia(UserSOData Userdata, int count)
    {
        Userdata.Dia += count;
    }
    public void InitDia(UserSOData Userdata)
    {
        Userdata.Dia = 900;
    }


    //유저가 소환할때 소모되는 골드
    public int GetUserSpawnGold(UserSOData Userdata)
    {
        return Userdata.UseGold;
    }
    //소환버튼눌렀을때 필요 골드가 증가함
    public void AddSpawnGold(UserSOData Userdata)
    {
        Userdata.UseGold += 2;
    }
    //골드 초기화
    public void Init_UseSpawnGold(UserSOData Userdata)
    {
        Userdata.UseGold = 20;
    }




    //유저가 보유골드
    public bool CanUseGold(UserSOData Userdata, int count)
    {
        int temp = Userdata.Gold - count;
        if (temp < 0)
        {
            return false; // 사용불가
        }
        else
        {
            return true;
        }
    }
    public void UseGold(UserSOData Userdata, int count)
    {
        bool usegold = CanUseGold(Userdata,count);
        if (usegold)
        {
            Userdata.Gold -= count;
        }
    }

    //골드 흭득할떄
    public void AddGold(UserSOData Userdata, int count)
    {
        Userdata.Gold += count;
    }
    public void InitGold(UserSOData Userdata)
    {
        Userdata.Gold = 100;
    }

    //인구수
    public void init_People(UserSOData Userdata)
    {
        Userdata.Currentpopulation = 0;
    }

    public bool CanAddPeople(UserSOData Userdata, int amount)
    {
        int temp = Userdata.Currentpopulation + amount;

        if (temp > Userdata.Maxpopulation)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void AddPeople(UserSOData Userdata, int count)
    {
        if (CanAddPeople(Userdata, count))
        {
            Userdata.Currentpopulation += count;
        }
    }

    public void UsePeople(UserSOData Userdata, int count)
    {
        Userdata.Currentpopulation -= count;

    }



    //확률로 등급 찾아 주기  노말 97% 레어 2% 희귀 0.7%, 신화 0.3%
    public Utill_Enum.Unit_Grade FInd_RandomUnitGrade()
    {
        float randomValue = Random.Range(0f, 100f); // 0 ~ 100 사이의 난수 생성

        if (randomValue < 97f)
            return Utill_Enum.Unit_Grade.Normal; // 97% 확률
        else if (randomValue < 99f)
            return Utill_Enum.Unit_Grade.Rare; // 2% 확률 (97 ~ 99)
        else if (randomValue < 99.7f)
            return Utill_Enum.Unit_Grade.Hero; // 0.7% 확률 (99 ~ 99.7)
        else
            return Utill_Enum.Unit_Grade.Myth; // 0.3% 확률 (99.7 ~ 100)
    }

    public UnitSO GetRandomUnitData(Utill_Enum.Unit_Grade grade) //등급에 맞는 랜덤 유닛 뽑기
    {
        UnitSO[] selectedUnitArray = null; // 선택된 등급의 유닛 배열
        //해당 등급 몬스터 배열에서 랜덤 데이터 뽑아내기
        // 등급에 따라 해당하는 배열 선택
        switch (grade)
        {
            case Utill_Enum.Unit_Grade.Normal:
                selectedUnitArray = NomarlUnitData;
                break;
            case Utill_Enum.Unit_Grade.Rare:
                selectedUnitArray = RareUnitData;
                break;
            case Utill_Enum.Unit_Grade.Hero:
                selectedUnitArray = HeroUnitData;
                break;
            case Utill_Enum.Unit_Grade.Myth:
                selectedUnitArray = MythUnitData;
                break;
        }

        // 선택된 배열이 비어있거나 유닛이 없으면 null 반환
        if (selectedUnitArray == null || selectedUnitArray.Length == 0)
            return null;

        // 해당 배열에서 랜덤하게 유닛 선택
        int randomIndex = Random.Range(0, selectedUnitArray.Length);
        return selectedUnitArray[randomIndex];
    }


    public UnitSO GetRandomUnitData()
    {
        Utill_Enum.Unit_Grade unitgrade = FInd_RandomUnitGrade();
        UnitSO[] selectedUnitArray = null; // 선택된 등급의 유닛 배열
        //해당 등급 몬스터 배열에서 랜덤 데이터 뽑아내기
        // 등급에 따라 해당하는 배열 선택
        switch (unitgrade)
        {
            case Utill_Enum.Unit_Grade.Normal:
                selectedUnitArray = NomarlUnitData;
                break;
            case Utill_Enum.Unit_Grade.Rare:
                selectedUnitArray = RareUnitData;
                break;
            case Utill_Enum.Unit_Grade.Hero:
                selectedUnitArray = HeroUnitData;
                break;
            case Utill_Enum.Unit_Grade.Myth:
                selectedUnitArray = MythUnitData;
                break;
        }

        // 선택된 배열이 비어있거나 유닛이 없으면 null 반환
        if (selectedUnitArray == null || selectedUnitArray.Length == 0)
            return null;

        // 해당 배열에서 랜덤하게 유닛 선택
        int randomIndex = Random.Range(0, selectedUnitArray.Length);
        return selectedUnitArray[randomIndex];
    }
}
