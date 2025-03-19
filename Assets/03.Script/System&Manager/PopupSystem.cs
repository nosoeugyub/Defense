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
        // ���� �˾��� ������ ���ο� �˾� ����
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

        //SoundManager.Instance.PlayAudio("UIClick"); ���� �Ŵ��� ����� ���
        // ���� �����ִ� �˾��̸� �ݱ�
        if (_currentPopup == _ipopup)
        {
            _currentPopup.Hide();
            _currentPopup = null;
            // ���� �˾� ǥ��
        }
        else
        {
            // ť���� �ش� �˾� ����
            _currentPopup.Hide();
            _popupQueue.Dequeue();
            //���ο��˾� ���
            _currentPopup = _ipopup;
        }
    }
}
