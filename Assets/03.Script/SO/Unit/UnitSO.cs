using UnityEngine;
using UnityEngine.UI;
//���� SO
[CreateAssetMenu(fileName = "NewUnit", menuName = "Unit/Unit")]
public class UnitSO : ScriptableObject
{
    public int Unit_id;//���� ���̵�
    public string UnitName; // ���� �̸�
    public int Attack; // ���ݷ�
    public float AttackSpeed; // ���� �ӵ�
    public int AttackCount; // �ִ� ���� ������ 
    public float AttackRange;// ���� �����Ÿ�

    public Utill_Enum.Unit_Grade UnitGrade;//���� ��� Ÿ��
    public Utill_Enum.Unit_AtkType Unit_AtkType;//���� ���� Ÿ��
    public Utill_Enum.Unit_Type UnitType;//���� ���� Ÿ��

    public int SellCount;//�ǸŰ���
    public Sprite UnitFaceImage;//���� �ʻ�Ȳ
}
