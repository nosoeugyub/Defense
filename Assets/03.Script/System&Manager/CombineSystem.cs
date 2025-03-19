using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineSystem : MonoBehaviour
{
    

    //조합이 가능한지 체크
    public bool CanTrySynthesis(Field slot) //필드에 조합 유닛이다있어야함
    {
        bool Possible = false;

        return Possible;
    }


    //조합하여 조합된 유닛 리턴
    public Unit PossibleSynthesis(Field slot)
    {
        Unit temp_unit = new Unit();

        return temp_unit;
    }


    //합성이 가능한지 체크
    public bool CanTryCombine(FieldSlot _fieldslot)//슬롯에서 3마리가 있어야함
    {
        bool Possible = false;

        return Possible;
    }

    //합성하여 합성된 유닛 리턴
    public Unit PossibleCombine(FieldSlot _fieldslot)
    {
        Unit temp_unit = new Unit();

        return temp_unit;
    }
}
