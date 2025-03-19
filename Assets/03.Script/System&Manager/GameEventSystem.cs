using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEventSystem 
{
    //디펜스 시작 시퀀스
    public delegate void GameSequence(Utill_Enum.Game_sequence Sequence);
    public static event GameSequence GameSequence_Event;
    public static void GameGameSequenceEvent(Utill_Enum.Game_sequence Sequence)
    {
        GameSequence_Event?.Invoke(Sequence);
    }

    //디펜스 게임오버 
    public delegate void GameOver();
    public static event GameOver GameOver_Event;
    public static void GameOverEvent()
    {
        GameOver_Event?.Invoke();
    }


    //적 죽음 이벤트
    public delegate void EnemyDie(bool isAi , int[] rewards , Enemy enemy );
    public static event EnemyDie EnemyDie_Event;
    public static void GameEnemyDie(bool isAi, int[] rewards ,  Enemy enemy )
    {
        EnemyDie_Event?.Invoke(isAi , rewards , enemy);
    }


    //유닛 소환 이벤트
    public delegate void SpawnUnit();
    public static event SpawnUnit SpawnUnit_Event;
    public static void GameSpawnUnitEvent()
    {
        SpawnUnit_Event?.Invoke();
    }

    //신화 유닛 소환 이벤트
    public delegate void SpawnMythUnit(MythUnitSO mythsodata);
    public static event SpawnMythUnit SpawnMythUnit_Event;
    public static void GameSpawnMythUnitEvent(MythUnitSO mythsodata)
    {
        SpawnMythUnit_Event?.Invoke(mythsodata);
    }


    //유닛 합성 이벤트
    public delegate void CombineUnit(List<Unit> unit , FieldSlot slot);
    public static event CombineUnit CombineUnit_Event;
    public static void GameCombineUnitEvent(List<Unit> unit , FieldSlot slot)
    {
        CombineUnit_Event?.Invoke(unit , slot);
    }

    //유닛 조합 이벤트
    public delegate void SynthesisUnit(List<MythUnitSO> unit);
    public static event SynthesisUnit SynthesisUnit_Event;
    public static void GameSynthesisUnitEvent(List<MythUnitSO> unit)
    {
        SynthesisUnit_Event?.Invoke(unit);
    }

    //유닛 조합 팝업 이벤트
    public delegate void SynthesispopupUnit();
    public static event SynthesispopupUnit SynthesispopupUnit_Event;
    public static void GameSynthesispopupUnitEvent()
    {
        SynthesispopupUnit_Event?.Invoke();
    }

    //유닛 판매 이벤트
    public delegate void SellUnit(FieldSlot slot);
    public static event SellUnit SellUnit_Event;
    public static void GameSellUnit_Event(FieldSlot slot)
    {
        SellUnit_Event?.Invoke(slot);
    }
}
