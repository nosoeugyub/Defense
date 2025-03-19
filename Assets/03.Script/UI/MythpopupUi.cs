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

    [SerializeField] private Image MythChartitleImgs; //�̸����� ���� �̹���
    [SerializeField] private TextMeshProUGUI Mythcharname; //ĳ���� �̸�
    [SerializeField] private TextMeshProUGUI Mythcherpersent; //ĳ���� �����

    [SerializeField] private Image[] MythCharNeedImgs; //�ʿ� ĳ�����̹���
    [SerializeField] private Image[] MythCharNeedCheckImgs; //üũ �̹���
    [SerializeField] private TextMeshProUGUI[] MythNeedtext; //���� �̺��� 

    [SerializeField] private Image MythCharNeedResultImg; //���� ��� �̹���

    [SerializeField] private Button MythSpawn;
    [SerializeField] private Button[] NowMythSpawnBtns; //��ü�ȯ ��ư!
    [SerializeField] private Image[] NowMythCharNeedImgs; //��ü�ȯ ĳ���� ��ư

    [SerializeField] private GameObject Mythcountbg;
    [SerializeField] private TextMeshProUGUI Mythcount; //��ȭ��ư ���� ������ ��ȭ ���հ��� ����

    private Dictionary<MythUnitSO, Button> activeMythSpawnButtons = new Dictionary<MythUnitSO, Button>();
    private int count = 0;//��ȭ ��� ���� ī��Ʈ
    private void Awake()
    {
        GameEventSystem.SynthesisUnit_Event += ActiveBtn; //������ �����Ҷ� ��� ��ȯ! ǥ�� ������ 
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
            // ����Ʈ ũ�� üũ (�����ϰ� ���� �������� ����)
            if (i >= NowMythSpawnBtns.Length || i >= NowMythCharNeedImgs.Length)
            {
                Debug.LogError($"NowMythSpawnBtns[{i}] �Ǵ� NowMythCharNeedImgs[{i}] ���� �Ұ�! ����Ʈ ũ�� �ʰ�");
                continue;
            }

            NowMythSpawnBtns[i].gameObject.SetActive(true);
            NowMythCharNeedImgs[i].sprite = unit[i].UnitResultFaceImage;

            // ��ư�� � ���ְ� ����Ǿ� �ִ��� ����
            activeMythSpawnButtons[unit[i]] = NowMythSpawnBtns[i];

            // ���� ������ ���� ��, ������ ���� ������� �̺�Ʈ �߰�
            NowMythSpawnBtns[i].onClick.RemoveAllListeners();

            MythUnitSO selectedUnit = unit[i]; 
            NowMythSpawnBtns[i].onClick.AddListener(() => SpawnMythUnit(selectedUnit));
        }
    }

    public void ButtonUpdate(MythUnitSO[] UnitDatas) //��ư �̹��� /�̸� ����
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

        //���� �ʿ� �̹��� �Ҵ� ��  ���� ���� üũ 
        //0.ī�װ� ���� ��ȭ������ ���൵�� ��ǥ���������
        ShowCategoryProgress(); // ī�װ��� ���൵ ǥ�� �Լ� ȣ��

        //1. �̹��� �Ҵ�
        for (int i = 0; i < mythdata.Mix_Units.Length; i++)
        {
            MythCharNeedImgs[i].sprite = mythdata.Mix_Units[i].UnitFaceImage;
        }

        //2. �ش� ���� ���ֵ��� �� �ʵ忡�ִ��� �˻�
        List<bool> isChecks = new List<bool>(mythdata.Mix_Units.Length);//���� ���� ������ŭ ����
        float currentProgress = 0; // ���� ���൵
        currentProgress = GetcurrentProgress(mythdata);

        // ���ֺ��� ���� ���θ� üũ�ϰ� ���൵�� ���
        for (int j = 0; j < mythdata.Mix_Units.Length; j++)
        {
            bool isCheckUnit = player._Field.TryCheckUnit(mythdata.Mix_Units[j]);

            // �ش� ������ �����Ǿ� ������
            if (isCheckUnit)
            {
                isChecks.Add(true);
            }
            else
            {
                isChecks.Add(false);
            }
        }


        //3. üũ �� ���� �̺��� �ؽ�Ʈ ��ȯ
        for (int i = 0; i < isChecks.Count; i++)
        {
            if (isChecks[i]) //���� �����ϰ� �ִٸ�
            {
                //�ش� �̸��� ������ ... üũ ������Ʈ �ѱ�
                MythCharNeedCheckImgs[i].gameObject.SetActive(true);
                MythNeedtext[i].text = "����";
            }
            else
            {
                //�ش� �̸��� �̺����� ... üũ ������Ʈ ��
                MythCharNeedCheckImgs[i].gameObject.SetActive(false);
                MythNeedtext[i].text = "�̺���";
            }
        }

   
        Mythcherpersent.text = "������" + $"{currentProgress}%";
        //��� �̹��� ����
        MythCharNeedResultImg.sprite = mythdata.UnitResultFaceImage;

        if (currentProgress >= 100) //�⿩���� ���� �ߴٸ�
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

    private void ShowCategoryProgress() // ī�װ��� ���൵�� ǥ��
    {
        // ī�װ��� ���൵�� ������ ����
        Dictionary<string, int> categoryProgress = new Dictionary<string, int>();

        // ī�װ��� ���൵�� ���
        for (int i = 0; i < DataManager.instance.MythUnitData.Length; i++)
        {
            float currentProgress = GetcurrentProgress(DataManager.instance.MythUnitData[i]);
             // ���൵�� 100%�� ���� �ʵ��� ����
             currentProgress = Mathf.Min(currentProgress, 100f);

            // ���൵ �ؽ�Ʈ ����
            MythCharbtntxts[i].text = "������: " + $"{currentProgress}%";
        }
    }

    //��ȭ�� �������� ���൵ ��ġȭ
    public float GetcurrentProgress(MythUnitSO sodata)
    {
        float temp = 0.0f;
        var mythUnit = sodata;

        // Mix_Units�� �� ���ֿ� ���� ����ġ�� ����
        Dictionary<Utill_Enum.Unit_Grade, float> gradeWeight = new Dictionary<Utill_Enum.Unit_Grade, float>(); // ����ġ ����� ��ųʸ�
        float totalWeight = 0f; // ��ü ����ġ �ջ�

        foreach (var unit in mythUnit.Mix_Units)
        {
            // �� ������ ��޿� �´� ����ġ�� ����
            float weight = unit.UnitGrade == Utill_Enum.Unit_Grade.Normal ? 1.0f :
                           (unit.UnitGrade == Utill_Enum.Unit_Grade.Rare ? 1.5f : 1.7f);

            // �ش� ����� ����ġ�� �� ���� �߰��ǵ��� ó��
            if (!gradeWeight.ContainsKey(unit.UnitGrade))
            {
                gradeWeight[unit.UnitGrade] = weight; // �ش� ��޿� �´� ����ġ�� ���ʷ� ����
            }

            totalWeight += weight; // ��ü ����ġ �ջ�
        }

        // �ش� ���� ���ֵ��� �� �ʵ忡 �ִ��� �˻�
        List<bool> isChecks = new List<bool>(mythUnit.Mix_Units.Length); // ���� ���� ������ŭ ����
        float currentProgress = 0f; // ���� ���൵
        float totalRequiredWeight = 0f;

        // ���ֺ��� ���� ���θ� üũ�ϰ� ���൵�� ���
        for (int j = 0; j < mythUnit.Mix_Units.Length; j++)
        {
            bool isCheckUnit = player._Field.TryCheckUnit(mythUnit.Mix_Units[j]);

            // �ش� ������ �����Ǿ� ������
            if (isCheckUnit)
            {
                isChecks.Add(true);

                // �ش� ������ ����ġ�� �����Ͽ� ���൵�� �⿩
                float unitWeight = gradeWeight[mythUnit.Mix_Units[j].UnitGrade];
                float unitProgress = (unitWeight / totalWeight) * 100f; // �ش� ������ �⿩���� ������ ���
                currentProgress += unitProgress;
            }
            else
            {
                isChecks.Add(false);
                totalRequiredWeight += 1.0f; // �ʿ� ���� �� ���� ������ ����, �ʿ� ����ġ�� �ø���.
            }
        }

        // ���൵�� 100%�� ���� �ʵ��� ����
        currentProgress = Mathf.Min(currentProgress, 100f);
        temp = currentProgress;
        return temp;
    }


    // ��ȭ ������ ��ȯ�ϴ� ��ư (��� ��ȯ ����)
    public void SpawnMythUnit(MythUnitSO UnitData)
    {
        count -= 1; // ī��Ʈ ����
        if (count <= 0)
        {
            Mythcountbg.gameObject.SetActive(false);
        }

        // �ش� ������ ��� ��ȯ ��ư ��Ȱ��ȭ
        if (activeMythSpawnButtons.TryGetValue(UnitData, out Button btn))
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
            activeMythSpawnButtons.Remove(UnitData);
        }

        // ��ȭ ���� ��ȯ �̺�Ʈ ȣ��
        GameEventSystem.GameSpawnMythUnitEvent(UnitData);
    }

    // �˾����� ��ȯ ��ư�� ������ ��
    public void SpawnMythUnitBtn(MythUnitSO UnitData)
    {
        count -= 1; // Ƚ�� ����
        if (count <= 0)
        {
            Mythcountbg.gameObject.SetActive(false);
        }

        // �˾� �ݱ�
        PopupSystem._instance.HidePop(PopupSystem._instance.Mythicmanager);

        // �ش� ������ ��� ��ȯ ��ư ��Ȱ��ȭ
        if (activeMythSpawnButtons.TryGetValue(UnitData, out Button btn))
        {
            btn.gameObject.SetActive(false);
            btn.onClick.RemoveAllListeners();
            activeMythSpawnButtons.Remove(UnitData);
        }

        // ��ȭ ���� ��ȯ �̺�Ʈ ȣ��
        GameEventSystem.GameSpawnMythUnitEvent(UnitData);
    }
}