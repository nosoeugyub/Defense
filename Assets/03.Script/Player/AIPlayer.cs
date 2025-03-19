using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : SummonerSystem
{



    private float checkInterval = 1f; // 1초마다 행동 체크
    private List<Unit> myUnits = new List<Unit>(); // AI가 보유한 유닛 리스트

    private void Awake()
    {
        GameEventSystem.GameSequence_Event += GameSequenceStart; //게임 시퀀스이벤트
    }

    void Start()
    {
        GameEventSystem.EnemyDie_Event += Rewardplayer;//몬스터가죽으면 해당 플레이어에게 돈지급
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
                StartCoroutine(AutoActionLoop()); // 주기적으로 행동 수행 (소환 및 합성)
                break;
            case Utill_Enum.Game_sequence.Stop:
                break;
        }
    }
    private void Rewardplayer(bool isAi, int[] rewards , Enemy enemy)
    {
        if (isAi)//플레이어가 잡은 몬스터
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
            TrySummon(); // 소환 시도
            
        }
    }



    private void TrySummon()
    {
        bool ai = true;//ai가 소환할때
        SummonUnit(ai);
        
    }
    

}
