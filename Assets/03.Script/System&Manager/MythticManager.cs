using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MythticManager : MonoBehaviour, IPopup
{
    [SerializeField] GameObject MythObj;
    [SerializeField] private MythpopupUi mythui;

    

    public void Close()
    {
        throw new System.NotImplementedException();
    }

    public void Hide()
    {
        MythObj.gameObject.SetActive(false);
    }

    public void Show()
    {
        //������Ʈ ����
        MythObj.gameObject.SetActive(true);

        init_Data();
    }

    private void init_Data()
    {
        //���� ���� ĭ�� Ŭ�� �� ������ ����
        PressButton(DataManager.instance.MythUnitData[0].Unit_id);
    }

    public void PressButton(int id)//�ش� �������� Unit_id�� �´� ������ ǥ��
    {
        // id�� �´� ������ �������� (�迭�� ���)
        MythUnitSO data = Array.Find(DataManager.instance.MythUnitData, c => c.Unit_id == id);
        mythui.ButtonUpdate(DataManager.instance.MythUnitData);
        if (data != null)
        {
            mythui.SeletButton(data); // �����Ͱ� ������ ��� UI ����
        }
        else
        {
            Debug.LogWarning($"ID {id}�� �ش��ϴ� �����Ͱ� �����ϴ�.");
        }
    }
}
