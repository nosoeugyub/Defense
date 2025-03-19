using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : SummonerSystem
{



    private float checkInterval = 1f; // 1�ʸ��� �ൿ üũ
    private List<Unit> myUnits = new List<Unit>(); // AI�� ������ ���� ����Ʈ

    private void Awake()
    {
        GameEventSystem.GameSequence_Event += GameSequenceStart; //���� �������̺�Ʈ
    }

    void Start()
    {
        GameEventSystem.EnemyDie_Event += Rewardplayer;//���Ͱ������� �ش� �÷��̾�� ������
    }
    private void GameSequenceStart(Utill_Enum.Game_sequence Sequence)
    {
        switch (Sequence)
        {
            case Utill_Enum.Game_sequence.DataLoad:
                break;
            case Utill_Enum.Game_sequence.Deley:
                break;
            case Utill_Enum.Game_sequence.Start:
                StartCoroutine(AutoActionLoop()); // �ֱ������� �ൿ ���� (��ȯ �� �ռ�)
                break;
            case Utill_Enum.Game_sequence.Stop:
                break;
        }
    }
    private void Rewardplayer(bool isAi, int[] rewards , Enemy enemy)
    {
        if (isAi)//�÷��̾ ���� ����
        {
            int count = rewards.Length;
            if (count == 1)
            {
                int goldamount = rewards[0];
                DataManager.instance.AddGold(Userdata, goldamount);
            }
            else
            {
                int goldamount = rewards[0];
                int diaamount = rewards[1];
                DataManager.instance.AddGold(Userdata, goldamount);
                DataManager.instance.AddDia(Userdata, diaamount);
            }
            
        }
    }


    IEnumerator AutoActionLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            TrySummon(); // ��ȯ �õ�
            
        }
    }



    private void TrySummon()
    {
        bool ai = true;//ai�� ��ȯ�Ҷ�
        SummonUnit(ai);
        
    }
    

}
