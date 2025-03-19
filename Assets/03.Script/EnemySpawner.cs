using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public bool isPlayerSpawner; // true면 플레이어 필드, false면 AI 필드

    public void SpawnMonster(string monsterName, PathEnemy assignedPath)
    {
        GameObject monsterObj = ObjectPooler.SpawnFromPool(monsterName, transform.position);
        if (monsterObj == null) return;

        Enemy monster = monsterObj.GetComponent<Enemy>();
        if (monster != null)
        {
            monster.SetPath(assignedPath); // 몬스터에 경로 설정
        }
    }
}
