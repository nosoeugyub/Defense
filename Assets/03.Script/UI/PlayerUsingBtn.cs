using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUsingBtn : MonoBehaviour
{
    //������ �÷��� ���� ��ư �������� ��ư��
    public Button SpawnUnitBtn; //��ȯ
    public Button UnitPossibleSynthesispopupBtn; //���� �˾� 
  //  public Button CombineUnitBtn; //�ռ�
   // public Button UnitPossibleSynthesisBtn;//���� ��ư

    private void Awake()
    {
        SpawnUnitBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
        UnitPossibleSynthesispopupBtn.onClick.AddListener(delegate { ClickPossibleSynthesisPopupButton(); });
        //  CombineUnitBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
        // UnitPossibleSynthesisBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
    }




    //��ȯ�̺�Ʈ
    public void ClickSpawnButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //����ؾ��� �̺�Ʈ // ��ȭ �̺�Ʈ , �α��� �̺�Ʈ , �ʵ� üũ -> ���ּ�ȯ �̺�Ʈ
    }

    //���� �˾� �̺�Ʈ
    public void ClickPossibleSynthesisPopupButton()
    {
       // GameEventSystem.GameSpawnUnitEvent();
        //����ؾ��� �̺�Ʈ // �˾� ���� //�˾��� �̿�� ������ �Ҵ�
    }

    //���� �̺�Ʈ
    public void ClickPossibleSynthesisButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //����ؾ��� �̺�Ʈ // ���տ� �ʿ��� ���� üũ // �α��� üũ //�ڸ� üũ
    }
    //�ռ� �̺�Ʈ
    public void ClickCombineButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //����ؾ��� �̺�Ʈ //�ռ��� �ʿ��� ���� üũ , �ռ����ʿ��� ��� üũ 
    }
}
