using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSlot : MonoBehaviour
{
    [SerializeField] private bool ai;
    public bool Ai
    {
        get { return ai; }
        set { ai = value; }
    }
    public GameObject SelectSlotObj;
    public Transform OneUnitPos;//��ĭ�� ������ �ϳ��϶� ��ġ
    public Transform[] TwoUnitPos; //��ĭ�� ������ �ΰ��϶� ��ġ
    public Transform[] ThreeUnitPos; //��ĭ�� ������ �����϶� ��ġ


    [SerializeField] private int slotUnitCount;
    public int SlotUnitCount
    {
        get { return slotUnitCount; }
        set { slotUnitCount = value;}
    }

    [SerializeField] private List<Unit> unitList;
    public List<Unit> UnitList
    {
        get { return unitList; }
        set { unitList = value;}
    }

    public void Init_Slot()
    {
        SlotUnitCount = 0;
        for (int i = 0; i < unitList.Count; i++)
        {
            unitList[i].gameObject.SetActive(false);
        }
        unitList.Clear();
    }

    public void Init_SlotCount()
    {
        SlotUnitCount = 0;
    }
    public int GetUnitCount(Unit unit)
    {
        return SlotUnitCount;
    }

    public void SetUnitCount(int count)
    {
        SlotUnitCount = count;
    }


    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    public bool CanAddUnit()
    {
        bool tmp_result = false;
        return tmp_result;
    }


    public void AddUnit(Unit unit)//������ �ʵ彽�Կ� ��ȯ
    {
        if (SlotUnitCount >= 3)
        {
            Debug.Log("�ִ� ���� ���� �����Ͽ� �߰��� �� �����ϴ�.");
            return;
        }

        // ���� �߰�
        unit.fieldSlot = this; // ���ֿ��� ���� �߰�
        unit.Setting();
        UnitList.Add(unit);
        SlotUnitCount++;
        UnitTransformPos(unit , SlotUnitCount);

        Debug.Log($"���� �߰� �Ϸ�. ���� ���� ���� ��: {SlotUnitCount}");
    }


    public void UnitTransformPos(Unit unit, int SlotUnitCount)
    {
        // ���� ���� ���� ���� ��ġ ��ġ ����
        switch (SlotUnitCount)
        {
            case 1:
                unit.transform.position = OneUnitPos.position;
                break;
            case 2:
                unit.transform.position = TwoUnitPos[0].position;
                UnitList[0].transform.position = TwoUnitPos[1].position; // ���� ���� ��ġ ����
                break;
            case 3:
                unit.transform.position = ThreeUnitPos[0].position;
                UnitList[0].transform.position = ThreeUnitPos[1].position;
                UnitList[1].transform.position = ThreeUnitPos[2].position;
                break;
        }
    }

    public void UnitTransformPos(int SlotUnitCount)
    {
        // ���� ���� ���� ���� ��ġ ��ġ ����
        switch (SlotUnitCount)
        {
            case 0:
                break;
            case 1:
                UnitList[0].transform.position = OneUnitPos.position;
                break;
            case 2:
                UnitList[0].transform.position = TwoUnitPos[0].position;
                UnitList[1].transform.position = TwoUnitPos[1].position; // ���� ���� ��ġ ����
                break;
            case 3:
                UnitList[0].transform.position = ThreeUnitPos[0].position;
                UnitList[1].transform.position = ThreeUnitPos[1].position;
                UnitList[2].transform.position = ThreeUnitPos[2].position;
                break;
        }
    }


    public bool CanMergeUnits()
    {
        // ���Կ� ������ ��Ȯ�� 3���� �ִ��� Ȯ��
        if (UnitList.Count != 3) return false;


        return true; // �ռ��� �����ϸ� true ��ȯ
    }

    
}
