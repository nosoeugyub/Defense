using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Field : MonoBehaviour
{
    public FieldSlot[] FieldSlotgrid; // �ʵ� ���� �迭


    //�ʵ��� ĳ���͸� ã�� ����� ����
    public void RemoveCharactor(UnitSO unit)
    {
        // 1. �ʵ忡�� unit�� ������ ���Ե��� ã��
        List<FieldSlot> matchingSlots = new List<FieldSlot>();

        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null)
            {
                foreach (var placedUnit in slot.UnitList)
                {
                    if (placedUnit.UnitSo == unit) // UnitSO ��
                    {
                        matchingSlots.Add(slot);
                        break; // �ش� ������ unit�� �����ϸ� ����Ʈ�� �߰��ϰ� ���� ���� �˻�
                    }
                }
            }
        }

        // 2. ������ ��ġ�� ������ ���ٸ� ����
        if (matchingSlots.Count == 0) return;

        // 3. �ش� ���� �� SlotUnitCount�� ���� ���� ���� ����
        FieldSlot targetSlot = matchingSlots.OrderBy(slot => slot.SlotUnitCount).First();

        // 4. ���õ� ���Կ��� unit ����
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
            unitToRemove.gameObject.SetActive(false); // ���� ��Ȱ��ȭ
            targetSlot.UnitList.Remove(unitToRemove); // ���� ����Ʈ���� ����
            targetSlot.SlotUnitCount--; // ���� �� ���� ���� ����

            //���� �ٽ� ��ġ
            targetSlot.UnitTransformPos(targetSlot.SlotUnitCount);
        }
    }


    //�ʵ忡 ������ �ִ��� Ȯ���ϴ� �Լ�
    public bool TryCheckUnit(UnitSO unit)
    {
        // 1.�̹� ��ġ�� ������ �ִ��� Ȯ��
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null)
            {
                // UnitList���� ���� SO�� ���� ������ �ִ��� Ȯ��
                foreach (var placedUnit in slot.UnitList)
                {
                    if (placedUnit.UnitSo == unit) // UnitSO ��
                    {
                        return true; // ������ ã��
                    }
                }
            }
        }

        // ������ ã�� ����
        return false;
    }

    public bool TryPlaceUnit(Unit unit)
    {
        // 1. �̹� ��ġ�� ������ �ִ��� Ȯ��
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null && slot.UnitList.Contains(unit))
            {
                // 2. �ش� ���� �� ���� ���� 3 �̸��̸� �߰� ��ġ + �ش罽�Գ� ��ȭ�����̸� �н�
                if (slot.SlotUnitCount < 3 && unit.UnitSo.UnitGrade != Utill_Enum.Unit_Grade.Myth)
                {
                    slot.AddUnit(unit);
                    return true; // ��ġ ����
                }
            }
        }

        // 3. �� ������ ����Ʈ�� ����
        List<FieldSlot> emptySlots = new List<FieldSlot>();
        foreach (var slot in FieldSlotgrid)
        {
            if (slot != null && slot.SlotUnitCount == 0)
            {
                emptySlots.Add(slot);
            }
        }

        // 4. �� ������ �ִٸ� ������ ���Կ� ��ġ
        if (emptySlots.Count > 0)
        {
            FieldSlot randomSlot = emptySlots[UnityEngine.Random.Range(0, emptySlots.Count)];
            randomSlot.AddUnit(unit);
            return true; // ������ �� ���Կ� ��ġ ����
        }

        return false; // ��ġ ���� (��� ������ ���� ��)
    }

    /// <summary>
    /// �ʵ��� ��� ������ ����Ʈ�� ��ȯ�ϴ� �Լ�
    /// </summary>
    /// <returns>�ʵ� �� ��� ���� ����Ʈ</returns>
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
