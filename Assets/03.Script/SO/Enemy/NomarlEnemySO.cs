using UnityEngine;

//�븻 ���� SO
[CreateAssetMenu(fileName = "NewNormalEnemy", menuName = "Enemy/NormalEnemy")]
public class NomarlEnemySO : ScriptableObject
{
    public string enemyName; // ���� �̸�
    public int maxHp; // �ִ� ü��
    public float moveSpeed; // �̵� �ӵ�
    public int physicalDefense; // ���� ����
    public int magicDefense; // ���� ����

    public int[] ShowStage; // ���� ��������
    public int[] RewordCoins; // ���� ���� 0..����....1...���̾�....

}
