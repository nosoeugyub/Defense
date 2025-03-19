using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LuckyPopup : MonoBehaviour , IPopup
{
    [SerializeField] GameObject LuckyObj;
    [SerializeField] private Luckypopupui Luckyui;

    public Button CloseBtn;
    private void Awake()
    {
        CloseBtn.onClick.AddListener(delegate { PopupSystem._instance.HidePop(this); });
    }

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public void Hide()
    {
        LuckyObj.gameObject.SetActive(false);
    }

    public void Show()
    {
        LuckyObj.gameObject.SetActive(true);
        Luckyui.SettingUi();
    }

    
}
