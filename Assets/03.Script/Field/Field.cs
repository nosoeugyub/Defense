using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    public FieldSlot[] FieldSlotgrid; // 필드 슬롯 배열


    //필드의 캐릭터를 찾고 지우는 로직
    public void RemoveCharactor(UnitSO unit)
    {
        // 1. 필드에서 unit을 포함한 슬롯들을 찾기
        List<FieldSlot> matchingSlots = new List<FieldSlot>();

        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null)
            {
                foreach (var placedUnit in slot.UnitList)
                {
                    if (placedUnit.UnitSo == unit) // UnitSO 비교
                    {
                        matchingSlots.Add(slot);
                        break; // 해당 슬롯이 unit을 포함하면 리스트에 추가하고 다음 슬롯 검사
                    }
                }
            }
        }

        // 2. 유닛이 배치된 슬롯이 없다면 리턴
        if (matchingSlots.Count == 0) return;

        // 3. 해당 슬롯 중 SlotUnitCount가 가장 작은 슬롯 선택
        FieldSlot targetSlot = matchingSlots.OrderBy(slot => slot.SlotUnitCount).First();

        // 4. 선택된 슬롯에서 unit 제거
        Unit unitToRemove = null;
        foreach (var placedUnit in targetSlot.UnitList)
        {
            if (placedUnit.UnitSo == unit)
            {
                unitToRemove = placedUnit;
                break;
            }
        }

        if (unitToRemove != null)
        {
            unitToRemove.gameObject.SetActive(false); // 유닛 비활성화
            targetSlot.UnitList.Remove(unitToRemove); // 슬롯 리스트에서 제거
            targetSlot.SlotUnitCount--; // 슬롯 내 유닛 개수 감소

            //유닛 다시 배치
            targetSlot.UnitTransformPos(targetSlot.SlotUnitCount);
        }
    }


    //필드에 유닛이 있는지 확인하는 함수
    public bool TryCheckUnit(UnitSO unit)
    {
        // 1.이미 배치된 유닛이 있는지 확인
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null)
            {
                // UnitList에서 유닛 SO가 같은 유닛이 있는지 확인
                foreach (var placedUnit in slot.UnitList)
                {
                    if (placedUnit.UnitSo == unit) // UnitSO 비교
                    {
                        return true; // 유닛을 찾음
                    }
                }
            }
        }

        // 유닛을 찾지 못함
        return false;
    }

    public bool TryPlaceUnit(Unit unit)
    {
        // 1. 이미 배치된 유닛이 있는지 확인
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null && slot.UnitList.Contains(unit))
            {
                // 2. 해당 슬롯 내 유닛 수가 3 미만이면 추가 배치 + 해당슬롯내 신화유닛이면 패스
                if (slot.SlotUnitCount < 3 && unit.UnitSo.UnitGrade != Utill_Enum.Unit_Grade.Myth)
                {
                    slot.AddUnit(unit);
                    return true; // 배치 성공
                }
            }
        }

        // 3. 빈 슬롯을 리스트에 저장
        List<FieldSlot> emptySlots = new List<FieldSlot>();
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null && slot.SlotUnitCount == 0)
            {
                emptySlots.Add(slot);
            }
        }

        // 4. 빈 슬롯이 있다면 랜덤한 슬롯에 배치
        if (emptySlots.Count > 0)
        {
            FieldSlot randomSlot = emptySlots[UnityEngine.Random.Range(0, emptySlots.Count)];
            randomSlot.AddUnit(unit);
            return true; // 랜덤한 빈 슬롯에 배치 성공
        }

        return false; // 배치 실패 (모든 슬롯이 가득 참)
    }

    /// <summary>
    /// 필드의 모든 슬롯을 리스트로 반환하는 함수
    /// </summary>
    /// <returns>필드 내 모든 슬롯 리스트</returns>
    public List<FieldSlot> GetAllSlots()
    {
        List<FieldSlot> allSlots = new List<FieldSlot>();

        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null)
            {
                allSlots.Add(slot);
            }
        }

        return allSlots;
    }
}
