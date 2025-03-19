using UnityEngine;

//노말 몬스터 SO
[CreateAssetMenu(fileName = "NewNormalEnemy", menuName = "Enemy/NormalEnemy")]
public class NomarlEnemySO : ScriptableObject
{
    public string enemyName; // 몬스터 이름
    public int maxHp; // 최대 체력
    public float moveSpeed; // 이동 속도
    public int physicalDefense; // 물리 방어력
    public int magicDefense; // 마법 방어력

    public int[] ShowStage; // 출현 스테이지
    public int[] RewordCoins; // 보상 코인 0..코인....1...다이아....

}
