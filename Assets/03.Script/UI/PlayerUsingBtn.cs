using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerUsingBtn : MonoBehaviour
{
    //유저가 플레이 도중 버튼 눌렀을때 버튼들
    public Button SpawnUnitBtn; //소환
    public Button UnitPossibleSynthesispopupBtn; //조합 팝업 
  //  public Button CombineUnitBtn; //합성
   // public Button UnitPossibleSynthesisBtn;//조합 버튼

    private void Awake()
    {
        SpawnUnitBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
        UnitPossibleSynthesispopupBtn.onClick.AddListener(delegate { ClickPossibleSynthesisPopupButton(); });
        //  CombineUnitBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
        // UnitPossibleSynthesisBtn.onClick.AddListener(delegate { ClickSpawnButton(); });
    }




    //소환이벤트
    public void ClickSpawnButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //등록해야할 이벤트 // 재화 이벤트 , 인구수 이벤트 , 필드 체크 -> 유닛소환 이벤트
    }

    //조합 팝업 이벤트
    public void ClickPossibleSynthesisPopupButton()
    {
       // GameEventSystem.GameSpawnUnitEvent();
        //등록해야할 이벤트 // 팝업 켜짐 //팝업에 이용될 데이터 할당
    }

    //조합 이벤트
    public void ClickPossibleSynthesisButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //등록해야할 이벤트 // 조합에 필요한 유닛 체크 // 인구수 체크 //자리 체크
    }
    //합성 이벤트
    public void ClickCombineButton()
    {
        GameEventSystem.GameSpawnUnitEvent();
        //등록해야할 이벤트 //합성에 필요한 조건 체크 , 합성에필요한 등급 체크 
    }
}
