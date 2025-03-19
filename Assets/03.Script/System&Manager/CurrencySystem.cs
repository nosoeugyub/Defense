using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
//������ ��ȭ�� ������Ʈ�ϴ� ��
public class CurrencySystem : MonoBehaviour
{
    public static CurrencySystem instance = null;

  

    [SerializeField] Player player;
    [SerializeField] AIPlayer AI;

    [SerializeField] TextMeshProUGUI GoldText;//��� ������Ʈ
    [SerializeField] TextMeshProUGUI DiaText; //���̾� ������Ʈ
    [SerializeField] TextMeshProUGUI PopulationText; //�α���������Ʈ
    [SerializeField] TextMeshProUGUI UseSpawnGold; //��ȯ����������Ʈ

    StringBuilder strbr = new StringBuilder();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        GameEventSystem.GameSequence_Event += GameSequenceStart; //���� �������̺�Ʈ
    }

    public RectTransform GetGoldTransform()
    {
        return GoldText.rectTransform;
    }
    private void GameSequenceStart(Utill_Enum.Game_sequence Sequence)
    {
        //���� �����ϸ� ���0 ���̾� 0 �α��� 0���� 
        switch (Sequence)
        {
            case Utill_Enum.Game_sequence.DataLoad:
                DataManager.instance.init_People(player.Userdata);
                DataManager.instance.InitDia(player.Userdata);
                DataManager.instance.InitGold(player.Userdata);
                DataManager.instance.Init_UseSpawnGold(player.Userdata);
                //ui������Ʈ
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
