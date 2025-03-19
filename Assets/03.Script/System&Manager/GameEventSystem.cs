using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameEventSystem 
{
    //���潺 ���� ������
    public delegate void GameSequence(Utill_Enum.Game_sequence Sequence);
    public static event GameSequence GameSequence_Event;
    public static void GameGameSequenceEvent(Utill_Enum.Game_sequence Sequence)
    {
        GameSequence_Event?.Invoke(Sequence);
    }

    //���潺 ���ӿ��� 
    public delegate void GameOver();
    public static event GameOver GameOver_Event;
    public static void GameOverEvent()
    {
        GameOver_Event?.Invoke();
    }


    //�� ���� �̺�Ʈ
    public delegate void EnemyDie(bool isAi , int[] rewards , Enemy enemy );
    public static event EnemyDie EnemyDie_Event;
    public static void GameEnemyDie(bool isAi, int[] rewards ,  Enemy enemy )
    {
        EnemyDie_Event?.Invoke(isAi , rewards , enemy);
    }


    //���� ��ȯ �̺�Ʈ
    public delegate void SpawnUnit();
    public static event SpawnUnit SpawnUnit_Event;
    public static void GameSpawnUnitEvent()
    {
        SpawnUnit_Event?.Invoke();
    }

    //��ȭ ���� ��ȯ �̺�Ʈ
    public delegate void SpawnMythUnit(MythUnitSO mythsodata);
    public static event SpawnMythUnit SpawnMythUnit_Event;
    public static void GameSpawnMythUnitEvent(MythUnitSO mythsodata)
    {
        SpawnMythUnit_Event?.Invoke(mythsodata);
    }


    //���� �ռ� �̺�Ʈ
    public delegate void CombineUnit(List<Unit> unit , FieldSlot slot);
    public static event CombineUnit CombineUnit_Event;
    public static void GameCombineUnitEvent(List<Unit> unit , FieldSlot slot)
    {
        CombineUnit_Event?.Invoke(unit , slot);
    }

    //���� ���� �̺�Ʈ
    public delegate void SynthesisUnit(List<MythUnitSO> unit);
    public static event SynthesisUnit SynthesisUnit_Event;
    public static void GameSynthesisUnitEvent(List<MythUnitSO> unit)
    {
        SynthesisUnit_Event?.Invoke(unit);
    }

    //���� ���� �˾� �̺�Ʈ
    public delegate void SynthesispopupUnit();
    public static event SynthesispopupUnit SynthesispopupUnit_Event;
    public static void GameSynthesispopupUnitEvent()
    {
        SynthesispopupUnit_Event?.Invoke();
    }

    //���� �Ǹ� �̺�Ʈ
    public delegate void SellUnit(FieldSlot slot);
    public static event SellUnit SellUnit_Event;
    public static void GameSellUnit_Event(FieldSlot slot)
    {
        SellUnit_Event?.Invoke(slot);
    }
}
