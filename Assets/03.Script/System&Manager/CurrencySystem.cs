using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
//유저의 재화를 업데이트하는 곳
public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance = null;

  

    [SerializeField] Player player;
    [SerializeField] AIPlayer AI;

    [SerializeField] TextMeshProUGUI GoldText;//골드 업데이트
    [SerializeField] TextMeshProUGUI DiaText; //다이아 업데이트
    [SerializeField] TextMeshProUGUI PopulationText; //인구수업데이트
    [SerializeField] TextMeshProUGUI UseSpawnGold; //소환사용골드업데이트

    StringBuilder strbr = new StringBuilder();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GameEventSystem.GameSequence_Event += GameSequenceStart; //게임 시퀀스이벤트
    }

    public RectTransform GetGoldTransform()
    {
        return GoldText.rectTransform;
    }
    private void GameSequenceStart(Utill_Enum.Game_sequence Sequence)
    {
        //게임 시작하면 골드0 다이아 0 인구수 0으로 
        switch (Sequence)
        {
            case Utill_Enum.Game_sequence.DataLoad:
                DataManager.instance.init_People(player.Userdata);
                DataManager.instance.InitDia(player.Userdata);
                DataManager.instance.InitGold(player.Userdata);
                DataManager.instance.Init_UseSpawnGold(player.Userdata);
                //ui업데이트
                UpdateSpawnGold(player.Userdata);
                UpdateGold(player.Userdata);
                UpdateDia(player.Userdata);
                UpdatePopulationText(player.Userdata);
                //ai
                DataManager.instance.init_People(AI.Userdata);
                DataManager.instance.InitDia(AI.Userdata);
                DataManager.instance.InitGold(AI.Userdata);
                DataManager.instance.Init_UseSpawnGold(AI.Userdata);
                break;
            case Utill_Enum.Game_sequence.Deley:
                break;
            case Utill_Enum.Game_sequence.Start:
                break;
            case Utill_Enum.Game_sequence.Stop:
                break;
            default:
                break;
        }
    }
    private void UnitCombineEvent(List<Unit> unit, FieldSlot slot)
    {
        throw new NotImplementedException();
    }
    private void SellUpdateGold(List<Unit> unit)
    {
        
    }

    public void UpdateSpawnGold(UserSOData Userdata)
    {
        
        int amount = Userdata.UseGold;
        UseSpawnGold.text = amount.ToString();
    }

    public void UpdateGold(UserSOData Userdata)
    {
        int amount = Userdata.Gold;
        GoldText.text = amount.ToString();
    }


    public void UpdateDia(UserSOData Userdata)
    {
        int amount = Userdata.Dia;
        DiaText.text = amount.ToString();
    }


    public void UpdatePopulationText(UserSOData Userdata)
    {
        int amount = Userdata.Currentpopulation;
        int maxamount = Userdata.Maxpopulation;

        strbr.Clear();
        strbr.Append(amount);
        strbr.Append("/");
        strbr.Append(maxamount);
        PopulationText.text = strbr.ToString();
    }
}
