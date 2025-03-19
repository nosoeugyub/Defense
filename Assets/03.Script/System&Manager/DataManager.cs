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

    public UnitSO[] NomarlUnitData; //�븻 ����
    public UnitSO[] RareUnitData; // ���� ����
    public UnitSO[] HeroUnitData;//��� ����
    public MythUnitSO[] MythUnitData;//��ȭ ����
    public BossEnemySO[] BossSo; //����������;
   
    //�����̸����� ���� �̹��� ��������
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


    //������ ���̾� �Ҹ��ҋ�
    public bool CanUseDia(UserSOData Userdata ,   int count)
    {
        int temp = Userdata.Dia - count;
        if (temp < 0)
        {
            return false; // ���Ұ�
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

    //���̾� ŉ���ҋ�
    public void AddDia(UserSOData Userdata, int count)
    {
        Userdata.Dia += count;
    }
    public void InitDia(UserSOData Userdata)
    {
        Userdata.Dia = 900;
    }


    //������ ��ȯ�Ҷ� �Ҹ�Ǵ� ���
    public int GetUserSpawnGold(UserSOData Userdata)
    {
        return Userdata.UseGold;
    }
    //��ȯ��ư�������� �ʿ� ��尡 ������
    public void AddSpawnGold(UserSOData Userdata)
    {
        Userdata.UseGold += 2;
    }
    //��� �ʱ�ȭ
    public void Init_UseSpawnGold(UserSOData Userdata)
    {
        Userdata.UseGold = 20;
    }




    //������ �������
    public bool CanUseGold(UserSOData Userdata, int count)
    {
        int temp = Userdata.Gold - count;
        if (temp < 0)
        {
            return false; // ���Ұ�
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

    //��� ŉ���ҋ�
    public void AddGold(UserSOData Userdata, int count)
    {
        Userdata.Gold += count;
    }
    public void InitGold(UserSOData Userdata)
    {
        Userdata.Gold = 100;
    }

    //�α���
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



    //Ȯ���� ��� ã�� �ֱ�  �븻 97% ���� 2% ��� 0.7%, ��ȭ 0.3%
    public Utill_Enum.Unit_Grade FInd_RandomUnitGrade()
    {
        float randomValue = Random.Range(0f, 100f); // 0 ~ 100 ������ ���� ����

        if (randomValue < 97f)
            return Utill_Enum.Unit_Grade.Normal; // 97% Ȯ��
        else if (randomValue < 99f)
            return Utill_Enum.Unit_Grade.Rare; // 2% Ȯ�� (97 ~ 99)
        else if (randomValue < 99.7f)
            return Utill_Enum.Unit_Grade.Hero; // 0.7% Ȯ�� (99 ~ 99.7)
        else
            return Utill_Enum.Unit_Grade.Myth; // 0.3% Ȯ�� (99.7 ~ 100)
    }

    public UnitSO GetRandomUnitData(Utill_Enum.Unit_Grade grade) //��޿� �´� ���� ���� �̱�
    {
        UnitSO[] selectedUnitArray = null; // ���õ� ����� ���� �迭
        //�ش� ��� ���� �迭���� ���� ������ �̾Ƴ���
        // ��޿� ���� �ش��ϴ� �迭 ����
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

        // ���õ� �迭�� ����ְų� ������ ������ null ��ȯ
        if (selectedUnitArray == null || selectedUnitArray.Length == 0)
            return null;

        // �ش� �迭���� �����ϰ� ���� ����
        int randomIndex = Random.Range(0, selectedUnitArray.Length);
        return selectedUnitArray[randomIndex];
    }


    public UnitSO GetRandomUnitData()
    {
        Utill_Enum.Unit_Grade unitgrade = FInd_RandomUnitGrade();
        UnitSO[] selectedUnitArray = null; // ���õ� ����� ���� �迭
        //�ش� ��� ���� �迭���� ���� ������ �̾Ƴ���
        // ��޿� ���� �ش��ϴ� �迭 ����
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

        // ���õ� �迭�� ����ְų� ������ ������ null ��ȯ
        if (selectedUnitArray == null || selectedUnitArray.Length == 0)
            return null;

        // �ش� �迭���� �����ϰ� ���� ����
        int randomIndex = Random.Range(0, selectedUnitArray.Length);
        return selectedUnitArray[randomIndex];
    }
}
