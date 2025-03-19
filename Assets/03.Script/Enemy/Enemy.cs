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

    // �ʱ�ȭ �޼ҵ�: �ܺο��� ȣ���Ͽ� �����͸� �ʱ�ȭ
    public void Init(NomarlEnemySO _Normalenemysodata)
    {
        // ���� �����͸� Normalenemysodata�� �Ҵ�
        Normalenemysodata = _Normalenemysodata;
       
    }

    // ��θ� �����ϰ�, ������ ��ġ�� ù ��° ��������Ʈ�� �̵�
    public void SetPath(PathEnemy newPath)
    {
        path = newPath;  // ��� �Ҵ�
        currentWaypointIndex = 0;  // ����� ù ��° ��������Ʈ���� ����
        transform.position = path.GetWaypoint(0);  // ù ��° ��������Ʈ ��ġ�� �̵�
        StartCoroutine(FollowPath());  // �ڷ�ƾ�� �����Ͽ� ��θ� ���󰡰� ��
    }

    // ��θ� ���� �̵��ϴ� �ڷ�ƾ
    private IEnumerator FollowPath()
    {
        while (true)
        {
            // ��ΰ� ������ ����
            if (path == null || path.WaypointCount == 0)
                yield break;

            // ���� ��ǥ ��ġ�� ������
            Vector3 targetPos = path.GetWaypoint(currentWaypointIndex);

            // ��ǥ ��ġ���� �̵�
            while (Vector3.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, Normalenemysodata.moveSpeed * Time.deltaTime);
                yield return null;
            }

            // ���� ��ǥ ��������Ʈ�� �Ѿ���� �ε����� �������� ���� ��������Ʈ�� �̵�
            currentWaypointIndex++;

            // �ε����� ���� �����ϸ� ó������ ���ư�
            if (currentWaypointIndex >= path.WaypointCount)
            {
                currentWaypointIndex = 0;
            }
        }
    }

    // �������� �޴� �޼���
    public void TakeDamage(int damage ,bool ai)
    {
        enemyRecovery.UpdateHpbar(damage, CurrentHp, Normalenemysodata.maxHp);
        CurrentHp -= damage;
        if (CurrentHp <= 0)
        {
            Die(ai);
        }
    }

    // ���� ����� �� ����
    private void Die(bool ai)
    {
        if (ai) //ai���� ������ ���ߴ����
        {
            GameEventSystem.GameEnemyDie(ai , Normalenemysodata.RewordCoins ,this);
        }
        else // �������� ������ ���ߴ����
        {
            GameEventSystem.GameEnemyDie(ai, Normalenemysodata.RewordCoins, this); ;
        }
       
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        //Ȱ��ȭ������ ����� ����
         CurrentHp = Normalenemysodata.maxHp;
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }

    void DeactiveDelay() => gameObject.SetActive(false);
}
