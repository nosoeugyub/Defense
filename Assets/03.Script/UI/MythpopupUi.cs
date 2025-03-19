using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;

public class MythpopupUi : MonoBehaviour
{
    public Player player;


    [SerializeField] private Image[] MythCharbtnImgs;
    [SerializeField] private TextMeshProUGUI[] MythCharbtntxts;

    [SerializeField] private Image MythChartitleImgs; //이름옆에 붙을 이미지
    [SerializeField] private TextMeshProUGUI Mythcharname; //캐릭터 이름
    [SerializeField] private TextMeshProUGUI Mythcherpersent; //캐릭터 진행률

    [SerializeField] private Image[] MythCharNeedImgs; //필요 캐릭터이미지
    [SerializeField] private Image[] MythCharNeedCheckImgs; //체크 이미지
    [SerializeField] private TextMeshProUGUI[] MythNeedtext; //보유 미보유 

    [SerializeField] private Image MythCharNeedResultImg; //조합 결과 이미지

    [SerializeField] private Button MythSpawn;
    [SerializeField] private Button[] NowMythSpawnBtns; //즉시소환 버튼!
    [SerializeField] private Image[] NowMythCharNeedImgs; //즉시소환 캐릭터 버튼

    [SerializeField] private GameObject Mythcountbg;
    [SerializeField] private TextMeshProUGUI Mythcount; //신화버튼 위에 나오는 신화 조합가능 갯수

    private Dictionary<MythUnitSO, Button> activeMythSpawnButtons = new Dictionary<MythUnitSO, Button>();
    private int count = 0;//신화 등급 조합 카운트
    private void Awake()
    {
        GameEventSystem.SynthesisUnit_Event += ActiveBtn; //조합이 가능할때 즉시 소환! 표시 떠야함 
    }

    private void ActiveBtn(List<MythUnitSO> unit)
    {
        if (unit == null || unit.Count == 0)
        {
            for (int i = 0; i < NowMythSpawnBtns.Length; i++)
            {
                NowMythSpawnBtns[i].gameObject.SetActive(false);
            }
             Mythcountbg.gameObject.SetActive(false);
            return;
        }

       

        count = unit.Count;
        Mythcountbg.gameObject.SetActive(true);
        Mythcount.text = count.ToString();

        for (int i = 0; i < count; i++)
        {
            // 리스트 크기 체크 (안전하게 범위 내에서만 접근)
            if (i >= NowMythSpawnBtns.Length || i >= NowMythCharNeedImgs.Length)
            {
                Debug.LogError($"NowMythSpawnBtns[{i}] 또는 NowMythCharNeedImgs[{i}] 접근 불가! 리스트 크기 초과");
                continue;
            }

            NowMythSpawnBtns[i].gameObject.SetActive(true);
            NowMythCharNeedImgs[i].sprite = unit[i].UnitResultFaceImage;

            // 버튼이 어떤 유닛과 연결되어 있는지 저장
            activeMythSpawnButtons[unit[i]] = NowMythSpawnBtns[i];

            // 기존 리스너 제거 후, 안전한 참조 방식으로 이벤트 추가
            NowMythSpawnBtns[i].onClick.RemoveAllListeners();

            MythUnitSO selectedUnit = unit[i]; 
            NowMythSpawnBtns[i].onClick.AddListener(() => SpawnMythUnit(selectedUnit));
        }
    }

    public void ButtonUpdate(MythUnitSO[] UnitDatas) //버튼 이미지 /이름 셋팅
    {
        for (int i = 0; i < UnitDatas.Length; i++)
        {
            MythCharbtnImgs[i].sprite = UnitDatas[i].UnitFaceImage;
        }
    }


    public void SeletButton(MythUnitSO mythdata)
    {
        MythChartitleImgs.sprite = mythdata.UnitFaceImage;
        Mythcharname.text = mythdata.UnitName;

        //조합 필요 이미지 할당 및  보유 한지 체크 
        //0.카테고리 고리의 신화유닛의 진행도는 다표시해줘야함
        ShowCategoryProgress(); // 카테고리별 진행도 표시 함수 호출

        //1. 이미지 할당
        for (int i = 0; i < mythdata.Mix_Units.Length; i++)
        {
            MythCharNeedImgs[i].sprite = mythdata.Mix_Units[i].UnitFaceImage;
        }

        //2. 해당 조건 유닛들이 내 필드에있는지 검사
        List<bool> isChecks = new List<bool>(mythdata.Mix_Units.Length);//조건 몬스터 갯수만큼 생성
        float currentProgress = 0; // 현재 진행도
        currentProgress = GetcurrentProgress(mythdata);

        // 유닛별로 보유 여부를 체크하고 진행도를 계산
        for (int j = 0; j < mythdata.Mix_Units.Length; j++)
        {
            bool isCheckUnit = player._Field.TryCheckUnit(mythdata.Mix_Units[j]);

            // 해당 유닛이 보유되어 있으면
            if (isCheckUnit)
            {
                isChecks.Add(true);
            }
            else
            {
                isChecks.Add(false);
            }
        }


        //3. 체크 및 보유 미보유 텍스트 변환
        for (int i = 0; i < isChecks.Count; i++)
        {
            if (isChecks[i]) //만약 보유하고 있다면
            {
                //해당 이름을 보유로 ... 체크 오브젝트 켜기
                MythCharNeedCheckImgs[i].gameObject.SetActive(true);
                MythNeedtext[i].text = "보유";
            }
            else
            {
                //해당 이름을 미보유로 ... 체크 오브젝트 끠
                MythCharNeedCheckImgs[i].gameObject.SetActive(false);
                MythNeedtext[i].text = "미보유";
            }
        }

   
        Mythcherpersent.text = "진행율" + $"{currentProgress}%";
        //결과 이미지 셋팅
        MythCharNeedResultImg.sprite = mythdata.UnitResultFaceImage;

        if (currentProgress >= 100) //기여도가 충족 했다면
        {
            MythSpawn.gameObject.SetActive(true);
            MythSpawn.onClick.AddListener(delegate { SpawnMythUnitBtn(mythdata); });
        }
        else
        {
            MythSpawn.gameObject.SetActive(false);
            MythSpawn.onClick.RemoveAllListeners();
        }
    }

    private void ShowCategoryProgress() // 카테고리별 진행도를 표기
    {
        // 카테고리별 진행도를 저장할 변수
        Dictionary<string, int> categoryProgress = new Dictionary<string, int>();

        // 카테고리별 진행도를 계산
        for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
        {
            float currentProgress = GetcurrentProgress(DataManager.instance.MythUnitData[i]);
             // 진행도가 100%를 넘지 않도록 제한
             currentProgress = Mathf.Min(currentProgress, 100f);

            // 진행도 텍스트 갱신
            MythCharbtntxts[i].text = "진행율: " + $"{currentProgress}%";
        }
    }

    //신화급 데이터의 진행도 수치화
    public float GetcurrentProgress(MythUnitSO sodata)
    {
        float temp = 0.0f;
        var mythUnit = sodata;

        // Mix_Units의 각 유닛에 대해 가중치를 설정
        Dictionary<Utill_Enum.Unit_Grade, float> gradeWeight = new Dictionary<Utill_Enum.Unit_Grade, float>(); // 가중치 저장용 딕셔너리
        float totalWeight = 0f; // 전체 가중치 합산

        foreach (var unit in mythUnit.Mix_Units)
        {
            // 각 유닛의 등급에 맞는 가중치를 설정
            float weight = unit.UnitGrade == Utill_Enum.Unit_Grade.Normal ? 1.0f :
                           (unit.UnitGrade == Utill_Enum.Unit_Grade.Rare ? 1.5f : 1.7f);

            // 해당 등급의 가중치가 한 번만 추가되도록 처리
            if (!gradeWeight.ContainsKey(unit.UnitGrade))
            {
                gradeWeight[unit.UnitGrade] = weight; // 해당 등급에 맞는 가중치를 최초로 설정
            }

            totalWeight += weight; // 전체 가중치 합산
        }

        // 해당 조건 유닛들이 내 필드에 있는지 검사
        List<bool> isChecks = new List<bool>(mythUnit.Mix_Units.Length); // 조건 몬스터 갯수만큼 생성
        float currentProgress = 0f; // 현재 진행도
        float totalRequiredWeight = 0f;

        // 유닛별로 보유 여부를 체크하고 진행도를 계산
        for (int j = 0; j < mythUnit.Mix_Units.Length; j++)
        {
            bool isCheckUnit = player._Field.TryCheckUnit(mythUnit.Mix_Units[j]);

            // 해당 유닛이 보유되어 있으면
            if (isCheckUnit)
            {
                isChecks.Add(true);

                // 해당 유닛의 가중치를 적용하여 진행도에 기여
                float unitWeight = gradeWeight[mythUnit.Mix_Units[j].UnitGrade];
                float unitProgress = (unitWeight / totalWeight) * 100f; // 해당 유닛의 기여도를 비율로 계산
                currentProgress += unitProgress;
            }
            else
            {
                isChecks.Add(false);
                totalRequiredWeight += 1.0f; // 필요 유닛 중 비보유 유닛을 세고, 필요 가중치를 늘린다.
            }
        }

        // 진행도가 100%를 넘지 않도록 제한
        currentProgress = Mathf.Min(currentProgress, 100f);
        temp = currentProgress;
        return temp;
    }


    // 신화 유닛을 소환하는 버튼 (즉시 소환 포함)
    public void SpawnMythUnit(MythUnitSO UnitData)
    {
        count -= 1; // 카운트 감소
        if (count <= 0)
        {
            Mythcountbg.gameObject.SetActive(false);
        }

        // 해당 유닛의 즉시 소환 버튼 비활성화
        if (activeMythSpawnButtons.TryGetValue(UnitData, out Button btn))
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
            activeMythSpawnButtons.Remove(UnitData);
        }

        // 신화 유닛 소환 이벤트 호출
        GameEventSystem.GameSpawnMythUnitEvent(UnitData);
    }

    // 팝업에서 소환 버튼을 눌렀을 때
    public void SpawnMythUnitBtn(MythUnitSO UnitData)
    {
        count -= 1; // 횟수 감소
        if (count <= 0)
        {
            Mythcountbg.gameObject.SetActive(false);
        }

        // 팝업 닫기
        PopupSystem._instance.HidePop(PopupSystem._instance.Mythicmanager);

        // 해당 유닛의 즉시 소환 버튼 비활성화
        if (activeMythSpawnButtons.TryGetValue(UnitData, out Button btn))
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
            activeMythSpawnButtons.Remove(UnitData);
        }

        // 신화 유닛 소환 이벤트 호출
        GameEventSystem.GameSpawnMythUnitEvent(UnitData);
    }
}