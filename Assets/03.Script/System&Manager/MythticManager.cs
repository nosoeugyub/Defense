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
        //오브젝트 켜짐
        MythObj.gameObject.SetActive(true);

        init_Data();
    }

    private void init_Data()
    {
        //제일 맨위 칸이 클릭 된 것으로 간주
        PressButton(DataManager.instance.MythUnitData[0].Unit_id);
    }

    public void PressButton(int id)//해당 데이터의 Unit_id에 맞는 데이터 표출
    {
        // id에 맞는 데이터 가져오기 (배열인 경우)
        MythUnitSO data = Array.Find(DataManager.instance.MythUnitData, c => c.Unit_id == id);
        mythui.ButtonUpdate(DataManager.instance.MythUnitData);
        if (data != null)
        {
            mythui.SeletButton(data); // 데이터가 존재할 경우 UI 갱신
        }
        else
        {
            Debug.LogWarning($"ID {id}에 해당하는 데이터가 없습니다.");
        }
    }
}
