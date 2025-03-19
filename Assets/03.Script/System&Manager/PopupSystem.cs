using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSystem : MonoBehaviour
{
    public static PopupSystem _instance = null;


    public MythticManager Mythicmanager;
    public LuckyPopup Luckypopup;

    public Button luckybtn;
    public Button Mythicbutton;
    

    private IPopup _currentPopup;
    private Queue<IPopup> _popupQueue = new Queue<IPopup>();

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        Init_Popup();
    }

    private void Init_Popup()
    {
        luckybtn.onClick.AddListener(() => ShowPopup(Luckypopup));
        Mythicbutton.onClick.AddListener(() => ShowPopup(Mythicmanager));
    }

    public void ShowPopup(IPopup _ipopup)
    {
       // SoundManager.Instance.PlayAudio("UIClick");
        // 현재 팝업이 없으면 새로운 팝업 열기
        if (_currentPopup == null)
        {
            _currentPopup = _ipopup;
            _popupQueue.Enqueue(_ipopup);
            _currentPopup.Show();
        }
        else if (_currentPopup == _ipopup)
        {
            _ipopup.Hide();
            _currentPopup = null;
        }
        else
        {
            HidePop(_ipopup);
            _popupQueue.Enqueue(_ipopup);
            _currentPopup.Show();
        }
    }

    public void HidePop(IPopup _ipopup)
    {

        //SoundManager.Instance.PlayAudio("UIClick"); 사운드 매니저 만들면 사용
        // 현재 열려있는 팝업이면 닫기
        if (_currentPopup == _ipopup)
        {
            _currentPopup.Hide();
            _currentPopup = null;
            // 다음 팝업 표시
        }
        else
        {
            // 큐에서 해당 팝업 제거
            _currentPopup.Hide();
            _popupQueue.Dequeue();
            //새로운팝업 등록
            _currentPopup = _ipopup;
        }
    }
}
