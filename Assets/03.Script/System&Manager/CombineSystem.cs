using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSystem : MonoBehaviour
{
    

    //������ �������� üũ
    public bool CanTrySynthesis(Field slot) //�ʵ忡 ���� �����̴��־����
    {
        bool Possible = false;

        return Possible;
    }


    //�����Ͽ� ���յ� ���� ����
    public Unit PossibleSynthesis(Field slot)
    {
        Unit temp_unit = new Unit();

        return temp_unit;
    }


    //�ռ��� �������� üũ
    public bool CanTryCombine(FieldSlot _fieldslot)//���Կ��� 3������ �־����
    {
        bool Possible = false;

        return Possible;
    }

    //�ռ��Ͽ� �ռ��� ���� ����
    public Unit PossibleCombine(FieldSlot _fieldslot)
    {
        Unit temp_unit = new Unit();

        return temp_unit;
    }
}
