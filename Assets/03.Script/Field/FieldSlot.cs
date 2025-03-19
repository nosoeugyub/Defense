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
    public Transform OneUnitPos;//한칸에 유닛이 하나일때 위치
    public Transform[] TwoUnitPos; //한칸에 유닛이 두개일때 위치
    public Transform[] ThreeUnitPos; //한칸에 유닛이 세개일때 위치


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


    public void AddUnit(Unit unit)//유닛을 필드슬롯에 소환
    {
        if (SlotUnitCount >= 3)
        {
            Debug.Log("최대 유닛 수에 도달하여 추가할 수 없습니다.");
            return;
        }

        // 유닛 추가
        unit.fieldSlot = this; // 유닛에도 슬롯 추가
        unit.Setting();
        UnitList.Add(unit);
        SlotUnitCount++;
        UnitTransformPos(unit , SlotUnitCount);

        Debug.Log($"유닛 추가 완료. 현재 슬롯 유닛 수: {SlotUnitCount}");
    }


    public void UnitTransformPos(Unit unit, int SlotUnitCount)
    {
        // 현재 유닛 수에 따라 배치 위치 설정
        switch (SlotUnitCount)
        {
            case 1:
                unit.transform.position = OneUnitPos.position;
                break;
            case 2:
                unit.transform.position = TwoUnitPos[0].position;
                UnitList[0].transform.position = TwoUnitPos[1].position; // 기존 유닛 위치 조정
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
        // 현재 유닛 수에 따라 배치 위치 설정
        switch (SlotUnitCount)
        {
            case 0:
                break;
            case 1:
                UnitList[0].transform.position = OneUnitPos.position;
                break;
            case 2:
                UnitList[0].transform.position = TwoUnitPos[0].position;
                UnitList[1].transform.position = TwoUnitPos[1].position; // 기존 유닛 위치 조정
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
        // 슬롯에 유닛이 정확히 3마리 있는지 확인
        if (UnitList.Count != 3) return false;


        return true; // 합성이 가능하면 true 반환
    }

    
}
