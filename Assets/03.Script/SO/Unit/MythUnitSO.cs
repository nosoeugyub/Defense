using UnityEngine;
using UnityEngine.UI;
//��ȭ ���� SO
[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/MythUnit")]
public class MythUnitSO : UnitSO
{
    public UnitSO[] Mix_Units;// ���տ� �ʿ��� id
    public Sprite UnitResultFaceImage;

}
//public int Unit_id;//���� ���̵�
//public string enemyName; // ���� �̸�
//public int Attack; // ���ݷ�
//public float AttackSpeed; // ���� �ӵ�


//public Utill_Enum.Unit_Grade UnitGrade;//���� ��� Ÿ��
//public Utill_Enum.Unit_AtkType Unit_AtkType;//���� ���� Ÿ��
//public Utill_Enum.Unit_Type UnitType;//���� ���� Ÿ��
// public int SellCount;//�ǸŰ���