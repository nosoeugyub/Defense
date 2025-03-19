using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System.Threading.Tasks;
using DG.Tweening;

public class Luckypopupui : MonoBehaviour
{
    [SerializeField] Player player;

    private float Normal = 60;
    private float Rare = 20;
    private float Hero = 10;

    [SerializeField] Image NormalActionImgs;
    [SerializeField] Image RareActionImgs;
    [SerializeField] Image HeroActionImgs;

    [SerializeField] TextMeshProUGUI NormalActiontxt;
    [SerializeField] TextMeshProUGUI RareActiontxt;
    [SerializeField] TextMeshProUGUI HeroActiontxt;

    [SerializeField] TextMeshProUGUI UserDia;
    [SerializeField] TextMeshProUGUI UserPeople;

    public Button LcukyNoramlBtn;
    public Button LcukyRareBtn;
    public Button LcukyHeroBtn;


    private void Awake()
    {
        LcukyNoramlBtn.onClick.AddListener(() => ClickLuckeyButton(Utill_Enum.Unit_Grade.Normal));
        LcukyRareBtn.onClick.AddListener(() => ClickLuckeyButton(Utill_Enum.Unit_Grade.Rare));
        LcukyHeroBtn.onClick.AddListener(() => ClickLuckeyButton(Utill_Enum.Unit_Grade.Hero));
    }

    public void SettingUi()
    {
        UserDia.text = player.Userdata.Dia.ToString();
        StringBuilder strbr = new StringBuilder();
        strbr.Append(player.Userdata.Currentpopulation);
        strbr.Append("/");
        strbr.Append(player.Userdata.Maxpopulation);
        UserPeople.text = strbr.ToString();
    }

    public void ClickLuckeyButton(Utill_Enum.Unit_Grade grade)
    {
        if (grade == Utill_Enum.Unit_Grade.Normal && 
            !DataManager.instance.CanUseDia(player.Userdata, 1) &&
            !DataManager.instance.CanAddPeople(player.Userdata, 1))//다이아 소모 안되면 리턴
        {
            return;
        }
        else if(grade == Utill_Enum.Unit_Grade.Rare && 
            !DataManager.instance.CanUseDia(player.Userdata, 1) &&
            !DataManager.instance.CanAddPeople(player.Userdata, 1))
        {
            return;
        }
        else if(grade == Utill_Enum.Unit_Grade.Hero 
            && !DataManager.instance.CanUseDia(player.Userdata, 2) &&
            !DataManager.instance.CanAddPeople(player.Userdata, 1))
        {
            return;
        }


        float result = Random.Range(0f, 100f);
        bool isSuccess = false;
        UnitSO data = null;
        Image ActionImgs = null;
        TextMeshProUGUI Actiontxt = null;
        switch (grade)
        {
            case Utill_Enum.Unit_Grade.Normal:
                if (result <= Normal)
                {
                    isSuccess = true;
                    data = DataManager.instance.GetRandomUnitData(Utill_Enum.Unit_Grade.Normal);
                }
                DataManager.instance.UseDia(player.Userdata, 1);
                ActionImgs = NormalActionImgs;
                Actiontxt = NormalActiontxt;
                break;
            case Utill_Enum.Unit_Grade.Rare:
                if (result <= Rare)
                {
                    isSuccess = true;
                    data = DataManager.instance.GetRandomUnitData(Utill_Enum.Unit_Grade.Rare);
                }
                DataManager.instance.UseDia(player.Userdata, 1);
                ActionImgs = RareActionImgs;
                Actiontxt = RareActiontxt;
                break;
            case Utill_Enum.Unit_Grade.Hero:
                if (result <= Hero)
                {
                    isSuccess = true;
                    data = DataManager.instance.GetRandomUnitData(Utill_Enum.Unit_Grade.Hero);
                }
                DataManager.instance.UseDia(player.Userdata, 2);
                ActionImgs = HeroActionImgs;
                Actiontxt = HeroActiontxt;
                break;
        }

        StartCoroutine(ShowLuckResult(isSuccess, data , ActionImgs , Actiontxt));
    }

    private IEnumerator ShowLuckResult(bool isSuccess, UnitSO data , Image actionImg, TextMeshProUGUI resultText)
    {
        resultText.gameObject.SetActive(false);
        actionImg.gameObject.SetActive(true);
        actionImg.transform.localScale = Vector3.one;

        // 1️⃣ 아이콘 회전하면서 점점 작아지는 연출
        actionImg.transform.DORotate(new Vector3(0, 0, -1080), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuart);
        actionImg.transform.DOScale(0.5f, 2f).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(2f);

        // "띠용" 효과 (커졌다 작아졌다 반복)
        actionImg.transform.DOScale(1.3f, 0.15f).SetEase(Ease.OutBounce)
            .OnComplete(() => actionImg.transform.DOScale(1f, 0.15f).SetEase(Ease.InBounce));

        yield return new WaitForSeconds(0.3f);

        // 3️⃣ 성공/실패 연출
        resultText.gameObject.SetActive(true);
        resultText.text = isSuccess ? "성공!" : "실패";
        resultText.transform.localScale = Vector3.zero;

        if (isSuccess)
        {
            // 성공 효과 (좌우 흔들기 + 텍스트 튀어나오기)
            actionImg.transform.DOShakePosition(0.5f, new Vector3(20f, 0, 0), 10, 90);
            resultText.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBounce)
                .OnComplete(() => resultText.transform.DOScale(1f, 0.1f).SetEase(Ease.InBounce));

            //소환
            player.SummonUnit(false, data);
        }
        else
        {
            // ❌ 실패 효과 (그냥 텍스트 튀어나오기)
            resultText.transform.DOScale(1.3f, 0.2f).SetEase(Ease.OutBounce)
                .OnComplete(() => resultText.transform.DOScale(1f, 0.1f).SetEase(Ease.InBounce));
        }
        SettingUi();
        CurrencySystem.instance.UpdateDia(player.Userdata);

        yield return new WaitForSeconds(2f);
        resultText.gameObject.SetActive(false);
    }

}