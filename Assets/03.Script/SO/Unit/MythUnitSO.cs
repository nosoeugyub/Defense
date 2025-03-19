using UnityEngine;
using UnityEngine.UI;
//신화 몬스터 SO
[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/MythUnit")]
public class MythUnitSO : UnitSO
{
    public UnitSO[] Mix_Units;// 조합에 필요한 id
    public Sprite UnitResultFaceImage;

}
//public int Unit_id;//유닛 아이디
//public string enemyName; // 유닛 이름
//public int Attack; // 공격력
//public float AttackSpeed; // 공격 속도


//public Utill_Enum.Unit_Grade UnitGrade;//유닛 등급 타입
//public Utill_Enum.Unit_AtkType Unit_AtkType;//유닛 공격 타입
//public Utill_Enum.Unit_Type UnitType;//유닛 종족 타입
// public int SellCount;//판매가격