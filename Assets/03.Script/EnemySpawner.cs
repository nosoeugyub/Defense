using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isPlayerSpawner; // true�� �÷��̾� �ʵ�, false�� AI �ʵ�

    public void SpawnMonster(string monsterName, PathEnemy assignedPath)
    {
        GameObject monsterObj = ObjectPooler.SpawnFromPool(monsterName, transform.position);
        if (monsterObj == null) return;

        Enemy monster = monsterObj.GetComponent<Enemy>();
        if (monster != null)
        {
            monster.SetPath(assignedPath); // ���Ϳ� ��� ����
        }
    }
}
