using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterWaveData", menuName = "Game/Monster Wave Data")]
public class MonsterWaveDataSO : ScriptableObject
{
    [System.Serializable]
    public class WaveInfo
    {
        public int Wave_index; //�������� �ε���
        public string monsterName; // ���� ����
        public int SpawnTime;          // ���� ������ �ð�
    }

    public List<WaveInfo> WaveList; //���̺� ����Ʈ
}
