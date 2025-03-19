using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventTrigger_Obj : EventTrigger
{
    public GameObject Dropobj = null;



    
    Camera uicam;
    private Transform tr;
    private Transform child_tr;
    private Vector3 orgscale;
    public Vector3 previousPos;     // 해당 오브젝트가 직전에 소속되어 있었던 부모 Transfron
    private bool buttonsActive = false; // 버튼이 활성화되었는지 체크
    private static FieldSlot currentSelectedSlot; // 현재 클릭된 타워 슬롯을 추적하기 위한 static 변수

    //event
    private EventTrigger.Entry entry_PointerClick;
    private EventTrigger.Entry entry_BeginDrag;
    private EventTrigger.Entry entry_OnDrag;
    private EventTrigger.Entry entry_EndDrag;
    private EventTrigger.Entry entry_pointerDown;
    private EventTrigger.Entry entry_PointerUp;


    private void Awake()
    {
        tr = transform;
        // child_tr = tr.GetChild(0);
        //  orgscale = child_tr.localScale;
        previousPos = tr.position;
       
       // uicam = GameObject.FindGameObjectWithTag("UICamera").GetComponent<Camera>();

        //: 3D eventtrigger eventlistener 등록 ...
        entry_PointerUp = new EventTrigger.Entry();
        entry_PointerUp.eventID = EventTriggerType.PointerUp;
        entry_PointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        this.triggers.Add(entry_PointerUp);

        entry_pointerDown = new EventTrigger.Entry();
        entry_pointerDown.eventID = EventTriggerType.PointerDown;
        entry_pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        this.triggers.Add(entry_pointerDown);


        //: 3D eventtrigger eventlistener 등록 ...
        entry_PointerClick = new EventTrigger.Entry();
        entry_PointerClick.eventID = EventTriggerType.PointerClick;
        entry_PointerClick.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });
        this.triggers.Add(entry_PointerClick);

        entry_BeginDrag = new EventTrigger.Entry();
        entry_BeginDrag.eventID = EventTriggerType.BeginDrag;
        entry_BeginDrag.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
        this.triggers.Add(entry_BeginDrag);

        entry_OnDrag = new EventTrigger.Entry();
        entry_OnDrag.eventID = EventTriggerType.Drag;
        entry_OnDrag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        this.triggers.Add(entry_OnDrag);

        entry_EndDrag = new EventTrigger.Entry();
        entry_EndDrag.eventID = EventTriggerType.EndDrag;
        entry_EndDrag.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
        this.triggers.Add(entry_EndDrag);
    }


    private void OnDisable()
    {

    }

    private void OnPointerUp(PointerEventData eventData)
    {

    }

    private void OnPointerDown(PointerEventData eventData)
    {
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        GameObject clickobj = eventData.pointerEnter;
        Debug.Log("클릭한 슬롯이름: " + clickobj.name);

        FieldSlot slot = clickobj.GetComponent<FieldSlot>();
        if (slot.Ai == true) //ai필드를 선택했다면....
        {
            UIPooling.Instance.HideAllButtons();
            //상대방 슬롯을 선택했을때... 처리 추가

            return;
        }

        if (slot == null) return;

        Unit currentunit = slot.UnitList.Count > 0 ? slot.UnitList[0] : null;
        int count = slot.SlotUnitCount;

        // 같은 슬롯을 다시 클릭하면 리턴
        if (slot == currentSelectedSlot)
            return;

        // 버튼을 풀에 반환하는 함수 호출 (기존 버튼 정리)
        UIPooling.Instance.HideAllButtons();
       

        if (currentunit == null) // 빈 슬롯이면 버튼 비활성화 후 종료
        {
            currentSelectedSlot = null;
            return;
        }

        // 새로운 슬롯 선택 처리
        currentSelectedSlot = slot;

        //월드 좌표 → 스크린 좌표 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(slot.transform.position);

        //스크린 좌표 → UI 로컬 좌표 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GameManager.instance.canvas.transform as RectTransform, screenPos, GameManager.instance.canvas.worldCamera, out Vector2 localPoint);


        // 판매 버튼 활성화 (윗쪽)
        if (count >= 1)
        {
            //사거리 표시 사정거리는 필드슬롯 을 기준으로 유닛의 사정거리만의 크기를 원 오브젝트로 보여줘야함
            ShowAttackRangeIndicator(slot, currentunit, localPoint);

            Transform sellButton = UIPooling.Instance.GetFromPool(UIPooling.Instance.SellButtons);
            
            if (sellButton != null)
            {
                //판메이벤트 
                sellButton.GetComponent<Button>().onClick.RemoveAllListeners(); // 기존 이벤트 모두 제거
                sellButton.GetComponent<Button>().onClick.AddListener(() => GameEventSystem.GameSellUnit_Event(slot)); // 복사한 리스트 전달


                sellButton.localPosition = localPoint + new Vector2(-20, 110); // 슬롯 위
                sellButton.gameObject.SetActive(true);
            }
            Transform combinButton = UIPooling.Instance.GetFromPool(UIPooling.Instance.CombinButton);
            if (combinButton != null)
            {
                combinButton.localPosition = localPoint + new Vector2(-25, -80); // 슬롯 아래
                //슬롯 비활성화
                combinButton.GetComponent<Button>().interactable = false;
                combinButton.GetComponent<Image>().color = Color.black;

                combinButton.gameObject.SetActive(true);
                // 합성 버튼 활성화 (아래쪽)
                if (count == 3)
                {
                    //합성슬롯 활성화 + 합성 이벤트 틍록
                    combinButton.GetComponent<Button>().interactable = true;
                    combinButton.GetComponent<Image>().color = Color.white;

                    //합성이벤트 전달
                    combinButton.GetComponent<Button>().onClick.RemoveAllListeners(); // 기존 이벤트 모두 제거
                    combinButton.GetComponent<Button>().onClick.AddListener(() => GameEventSystem.GameCombineUnitEvent(slot.UnitList , slot)); // 복사한 리스트 전달
                }
            }
    
        }

        

    }


    private void ShowAttackRangeIndicator(FieldSlot slot , Unit unit , Vector2 localPoint)
    {
        if (unit == null)
        {
            return;
        }

        //사정거리 원을 풀에서 가져옴 
        Transform rangelindicator = UIPooling.Instance.GetFromPool(UIPooling.Instance.AttackRange);
        rangelindicator.gameObject.SetActive(true);
        rangelindicator.transform.localPosition = localPoint;
        // 사정거리 크기 설정(OverlapSphere와 같은 크기)
        float rangeSize = unit.UnitSo.AttackRange * 3f; // 실제 사정거리 크기 (유닛 SO의 attackRange 값을 사용)
        // UI 크기 변경: 원을 그릴 수 있도록 UI RectTransform 크기를 조정
        rangelindicator.GetComponent<RectTransform>().localScale = new Vector2(rangeSize, rangeSize);  // UI의 크기를 공격 범위에 맞게 설정
      
    }

    private void OnBeginDrag(PointerEventData eventData)
    {

    }


    private void OnDrag(PointerEventData eventData)
    {
        return;

        Debug.Log("드래그 중");

        // 마우스 좌표를 월드 좌표로 변환
        Vector3 worldPosition = uicam.ScreenToWorldPoint(Input.mousePosition);

        // 마우스 좌표가 캔버스 안에 있는지 여부 판단
        if (IsMouseInCanvas(worldPosition))
        {
            Debug.Log("캔버스 내부");

            // 슬롯이 속한 Canvas의 RectTransform을 기준으로 변환
            RectTransform canvasRT = GameManager.instance.canvas.transform as RectTransform;
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRT, Input.mousePosition, uicam, out localPos);
            Debug.Log("localPos" + localPos);
            tr.position = canvasRT.TransformPoint(localPos);
        }
        else
        {
            Debug.Log("월드 좌표");
            // 캔버스 밖이면 월드 좌표로 설정
            tr.position = worldPosition;
        }

    }

    private void OnEndDrag(PointerEventData eventData)
    {
        return;

        Debug.Log("드래그 끝");
        Dropobj = eventData.pointerCurrentRaycast.gameObject;

        Debug.Log(Dropobj.gameObject.name);
    }

    // 마우스 좌표가 캔버스 안에 있는지 여부를 판단하는 함수
    private bool IsMouseInCanvas(Vector3 mousePosition)
    {
        return false;

        RectTransform rt = GameManager.instance.canvas.transform as RectTransform;
        return RectTransformUtility.RectangleContainsScreenPoint(rt, mousePosition);
    }


    private void CreateButtons()
    {


        buttonsActive = true;
    }

    private void RemoveButtons()
    {

        buttonsActive = false;
    }

    // 조합 버튼 클릭 시 실행되는 함수
    private void OnCombineButtonClick()
    {
        Debug.Log("조합 버튼 클릭됨!");
        // 여기에 조합 로직 추가
    }

    // 판매 버튼 클릭 시 실행되는 함수
    private void OnSellButtonClick()
    {
        Debug.Log("판매 버튼 클릭됨!");
        // 여기에 판매 로직 추가
    }
}
