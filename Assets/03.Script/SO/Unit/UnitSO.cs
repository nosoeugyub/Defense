using UnityEngine;
using UnityEngine.UI;
//유닛 SO
[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/Unit")]
public class UnitSO : ScriptableObject
{
    public int Unit_id;//유닛 아이디
    public string UnitName; // 유닛 이름
    public int Attack; // 공격력
    public float AttackSpeed; // 공격 속도
    public int AttackCount; // 최대 공격 마리수 
    public float AttackRange;// 몬스터 사정거리

    public Utill_Enum.Unit_Grade UnitGrade;//유닛 등급 타입
    public Utill_Enum.Unit_AtkType Unit_AtkType;//유닛 공격 타입
    public Utill_Enum.Unit_Type UnitType;//유닛 종족 타입

    public int SellCount;//판매가격
    public Sprite UnitFaceImage;//유닛 초상황
}
