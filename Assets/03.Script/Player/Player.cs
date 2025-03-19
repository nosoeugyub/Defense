using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameEventSystem;
using DG.Tweening;

public class Player : SummonerSystem
{

    private void Start()
    {
        GameEventSystem.SpawnUnit_Event += ClickSpawnBtn;//소환버튼눌렀을때
        GameEventSystem.SellUnit_Event += UnitSellEvent; //판매버튼눌렀을때
        GameEventSystem.CombineUnit_Event += UnitCombineEvent; //합성버튼눌렀을때
        GameEventSystem.SpawnMythUnit_Event += MythUnitSpawn; // 즉시 소환이나 테벵서 버튼 소환을 했을때
        GameEventSystem.EnemyDie_Event += Rewardplayer;//몬스터가죽으면 해당 플레이어에게 돈지급
    }

    private void UnitCombineEvent(List<Unit> unit, FieldSlot slot)
    {
        bool isAi = false; //플레이어가 직접 합성
        UnitCombine(unit, slot, isAi);
    }

    private void Rewardplayer(bool isAi, int[] rewards , Enemy enemy)
    {
        if (!isAi)//플레이어가 잡은 몬스터
        {
            int count = rewards.Length;

            if (count == 1)
            {
                int goldamount = rewards[0];
                DataManager.instance.AddGold(Userdata, goldamount);
                //골드 소유창 업데이트
                CurrencySystem.instance.UpdateGold(Userdata);

                // 🔹 골드 UI 가져오기 (골드 아이콘 & 텍스트)
                RectTransform goldStartPos = CurrencySystem.instance.GetGoldTransform();
                Transform Gold = UIPooling.Instance.GetFromPool(UIPooling.Instance.CoinUi);
                Transform Notice = UIPooling.Instance.GetFromPool(UIPooling.Instance.Noticetxt);

                TextMeshProUGUI noticeText = Notice.GetComponent<TextMeshProUGUI>();
                noticeText.text = $"+{goldamount}";

                // Gold와 Notice가 같은 부모를 가지고 있어야 상대적인 위치 계산이 정확함
                Gold.SetParent(goldStartPos.parent);
                Notice.SetParent(goldStartPos.parent);

                // goldStartPos에서 상대적인 위치로 이동
                Gold.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition + new Vector3(-80, 0, 0);
                Notice.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition;

                // UI 활성화
                Gold.gameObject.SetActive(true);
                Notice.gameObject.SetActive(true);

                Vector3 goldEndPos = goldStartPos.localPosition + new Vector3(-80, 100, 0);
                Vector3 noticeEndPos = goldStartPos.localPosition + new Vector3(0, 100, 0);

                Gold.GetComponent<RectTransform>().DOJumpAnchorPos(goldEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    Gold.gameObject.SetActive(false);
                });

                Notice.GetComponent<RectTransform>().DOJumpAnchorPos(noticeEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    Notice.gameObject.SetActive(false);
                });
            }
            else if(count == 2)
            {
                //골드 먼저
                int goldamount = rewards[0];
                DataManager.instance.AddGold(Userdata, goldamount);
                //골드 소유창 업데이트
                CurrencySystem.instance.UpdateGold(Userdata);

                // 🔹 골드 UI 가져오기 (골드 아이콘 & 텍스트)
                RectTransform goldStartPos = CurrencySystem.instance.GetGoldTransform();
                Transform Gold = UIPooling.Instance.GetFromPool(UIPooling.Instance.CoinUi);
                Transform Notice = UIPooling.Instance.GetFromPool(UIPooling.Instance.Noticetxt);

                TextMeshProUGUI noticeText = Notice.GetComponent<TextMeshProUGUI>();
                noticeText.text = $"+{goldamount}";

                // Gold와 Notice가 같은 부모를 가지고 있어야 상대적인 위치 계산이 정확함
                Gold.SetParent(goldStartPos.parent);
                Notice.SetParent(goldStartPos.parent);

                // goldStartPos에서 상대적인 위치로 이동
                Gold.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition + new Vector3(-80, 0, 0);
                Notice.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition;

                // UI 활성화
                Gold.gameObject.SetActive(true);
                Notice.gameObject.SetActive(true);

                Vector3 goldEndPos = goldStartPos.localPosition + new Vector3(-80, 100, 0);
                Vector3 noticeEndPos = goldStartPos.localPosition + new Vector3(0, 100, 0);

                Gold.GetComponent<RectTransform>().DOJumpAnchorPos(goldEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    Gold.gameObject.SetActive(false);
                });

                Notice.GetComponent<RectTransform>().DOJumpAnchorPos(noticeEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    Notice.gameObject.SetActive(false);
                });


                //다이아도
                int Diamount = rewards[1];
                DataManager.instance.AddGold(Userdata, Diamount);
                //골드 소유창 업데이트
                CurrencySystem.instance.UpdateGold(Userdata);

                // 🔹 골드 UI 가져오기 (골드 아이콘 & 텍스트)
                RectTransform diaStartPos = CurrencySystem.instance.GetGoldTransform();
                Transform Dia = UIPooling.Instance.GetFromPool(UIPooling.Instance.CoinUi);
                Transform DiaNotice = UIPooling.Instance.GetFromPool(UIPooling.Instance.Noticetxt);

                TextMeshProUGUI DianoticeText = DiaNotice.GetComponent<TextMeshProUGUI>();
                noticeText.text = $"+{goldamount}";

                // Gold와 Notice가 같은 부모를 가지고 있어야 상대적인 위치 계산이 정확함
                Dia.SetParent(goldStartPos.parent);
                DiaNotice.SetParent(goldStartPos.parent);

                // goldStartPos에서 상대적인 위치로 이동
                Dia.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition + new Vector3(-80, 0, 0);
                DiaNotice.GetComponent<RectTransform>().localPosition = goldStartPos.localPosition;

                // UI 활성화
                Dia.gameObject.SetActive(true);
                DiaNotice.gameObject.SetActive(true);

                Vector3 DiaEndPos = goldStartPos.localPosition + new Vector3(-80, 100, 0);
                Vector3 DianoticeEndPos = goldStartPos.localPosition + new Vector3(0, 100, 0);

                Dia.GetComponent<RectTransform>().DOJumpAnchorPos(DiaEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    Dia.gameObject.SetActive(false);
                });

                DiaNotice.GetComponent<RectTransform>().DOJumpAnchorPos(DianoticeEndPos, 10f, 1, 0.3f).OnComplete(() =>
                {
                    DiaNotice.gameObject.SetActive(false);
                });
            }
            
        }
    }

    private void MythUnitSpawn(MythUnitSO mythsodata)
    {
        bool ai = false;
        MythUnitSpawnField(mythsodata , ai);
    }

   

    private void UnitSellEvent(FieldSlot slot)
    {
        SellUnit(slot);
        //ui도 없데이트
        CurrencySystem.instance.UpdatePopulationText(Userdata);
        CurrencySystem.instance.UpdateGold(Userdata);

        for (int i = 0; i < slot.UnitList.Count; i++)
        {
            slot.UnitList[i].gameObject.SetActive(false);//유닛 삭제
        }
        //버튼들도 다시 비활성화
        UIPooling.Instance.HideAllButtons();
    }

    private void ClickSpawnBtn()
    {
        bool ai = false;//플레이어가 소환할때
        SummonUnit(ai);
    }

    private void AddUseGold()//소환할떄마다 2골드 씩증가
    {
        Userdata.UseGold += 2;
    }

    private void InitUseGold()//소환소모재화 초기화
    {
        Userdata.UseGold = 20;
    }

}
