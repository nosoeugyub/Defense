using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private PathEnemy path;
    private int currentWaypointIndex = 0;

    [SerializeField] private EnemyREcovery enemyRecovery;
    [SerializeField] private NomarlEnemySO normalenemysodata;
    public NomarlEnemySO Normalenemysodata
    {
        get { return normalenemysodata; }
        set { normalenemysodata = value; }
    }

    float CurrentHp;

    // 초기화 메소드: 외부에서 호출하여 데이터를 초기화
    public void Init(NomarlEnemySO _Normalenemysodata)
    {
        // 받은 데이터를 Normalenemysodata에 할당
        Normalenemysodata = _Normalenemysodata;
       
    }

    // 경로를 설정하고, 몬스터의 위치를 첫 번째 웨이포인트로 이동
    public void SetPath(PathEnemy newPath)
    {
        path = newPath;  // 경로 할당
        currentWaypointIndex = 0;  // 경로의 첫 번째 웨이포인트부터 시작
        transform.position = path.GetWaypoint(0);  // 첫 번째 웨이포인트 위치로 이동
        StartCoroutine(FollowPath());  // 코루틴을 실행하여 경로를 따라가게 함
    }

    // 경로를 따라 이동하는 코루틴
    private IEnumerator FollowPath()
    {
        while (true)
        {
            // 경로가 없으면 종료
            if (path == null || path.WaypointCount == 0)
                yield break;

            // 현재 목표 위치를 가져옴
            Vector3 targetPos = path.GetWaypoint(currentWaypointIndex);

            // 목표 위치까지 이동
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Normalenemysodata.moveSpeed * Time.deltaTime);
                yield return null;
            }

            // 현재 목표 웨이포인트를 넘어갔으면 인덱스를 증가시켜 다음 웨이포인트로 이동
            currentWaypointIndex++;

            // 인덱스가 끝에 도달하면 처음으로 돌아감
            if (currentWaypointIndex >= path.WaypointCount)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    // 데미지를 받는 메서드
    public void TakeDamage(int damage ,bool ai)
    {
        enemyRecovery.UpdateHpbar(damage, CurrentHp, Normalenemysodata.maxHp);
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Die(ai);
        }
    }

    // 적이 사망할 때 실행
    private void Die(bool ai)
    {
        if (ai) //ai한테 죽임을 당했더라면
        {
            GameEventSystem.GameEnemyDie(ai , Normalenemysodata.RewordCoins ,this);
        }
        else // 유저한테 죽임을 당했더라면
        {
            GameEventSystem.GameEnemyDie(ai, Normalenemysodata.RewordCoins, this); ;
        }
       
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //활성화됐을떄 사용할 로직
         CurrentHp = Normalenemysodata.maxHp;
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    void DeactiveDelay() => gameObject.SetActive(false);
}
