using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWaveData", menuName = "Game/Monster Wave Data")]
public class MonsterWaveDataSO : ScriptableObject
{
    [System.Serializable]
    public class WaveInfo
    {
        public int Wave_index; //스테이지 인덱스
        public string monsterName; // 몬스터 종류
        public int SpawnTime;          // 몬스터 나오는 시간
    }

    public List<WaveInfo> WaveList; //웨이브 리스트
}
