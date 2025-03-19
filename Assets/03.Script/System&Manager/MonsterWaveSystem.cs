using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static GameEventSystem;
using System;
using System.Text;
public class MonsterWaveSystem : MonoBehaviour
{
    public GameObject BossAramObj;
    public Image BossImg;
    public TextMeshProUGUI BossAramText;
    public GameObject ClearBossObj;

    public static MonsterWaveSystem instance = null;
    public static int bossStack = 0;






    public static int CurrnetEnemyCount;//몬스터 현재 마릿수
    public static readonly int MaxEnemyCount = 100;//몬스터 최대치 마릿수

    [SerializeField] private GameObject[] CountDownTextObj;//현제 웨이브 표시
    [SerializeField] private TextMeshProUGUI[] CountDownText;//현제 웨이브 표시

    [SerializeField] private TextMeshProUGUI WaveText;//현제 웨이브 표시
    [SerializeField] private TextMeshProUGUI WaveTime; //현제 남은 시간 표시
    [SerializeField] private TextMeshProUGUI currentEnemyCount; //현제 소환된 몬스터 표시

    [SerializeField] private Image EnemyPrograssPersentbar;

    public float startDelay = 5f;
    public float spawnInterval = 3f;

    public EnemySpawner playerSpawner; // 플레이어 필드에서 몬스터 소환
    public EnemySpawner aiSpawner;     // AI 필드에서 몬스터 소환
    public PathEnemy playerPath;
    public PathEnemy aiPath;

    public MonsterWaveDataSO waveData; // SO 데이터 참조

    private int currentWaveIndex = 0; // 현재 진행 중인 웨이브
    private bool isWaveActive = false; // 웨이브 진행 중 여부






    public void Awake()
    {
        if (instance == null)
        {

            instance = this;
        }
        GameEventSystem.GameSequence_Event += GameSequenceFnc;
        GameEventSystem.EnemyDie_Event += DeathCount;//몬스터가죽으면 해당 플레이어에게 돈지급
        GameEventSystem.EnemyDie_Event += EnemyDieEvent;
    }


    private void EnemyDieEvent(bool isAi, int[] rewards, Enemy enemy)
    {
        if (enemy.Normalenemysodata.name.Contains("Boss")) // 이름에 "Boss" 포함 여부 확인
        {
            bossStack++; // bossStack 증가

            if (bossStack >= 2) // 스택이 2개가 되면
            {
                ClearBossObj.gameObject.SetActive(false);

                // 1초 후 오브젝트 비활성화
                Invoke(nameof(HideBossAram), 1f);

                bossStack = 0;
            }
        }
    }

    public void ClearBoss()
    {
        ClearBossObj.gameObject.SetActive(false);
    }

    public void ActiveBoss(string MonsterName, int Wave, int LimitTime)
    {
        BossAramObj.gameObject.SetActive(true);
        StringBuilder stb = new StringBuilder();

        BossImg.sprite = DataManager.instance.GetBossSprite(MonsterName);
        stb.Append("Wave" + Wave);
        stb.AppendLine("\"" + MonsterName + "\" 보스 등장!!");
        stb.AppendLine("제한 시간" + LimitTime);

        // 1초 후에 오브젝트 비활성화
        Invoke(nameof(HideBossAram), 1f);
    }

    private void HideBossAram()
    {
        BossAramObj.gameObject.SetActive(false);
    }

    private void DeathCount(bool isAi, int[] rewards , Enemy enemy)
    {
        OnMonsterDeath();
    }

    private void GameSequenceFnc(Utill_Enum.Game_sequence Sequence)
    {
        switch (Sequence)
        {
            case Utill_Enum.Game_sequence.DataLoad:
                Init_WaveUi();
                break;
            case Utill_Enum.Game_sequence.Deley:
                //대진 연출 나오면서

                //카운트다운 시작...
                SpawnWaveStart();

                break;
            case Utill_Enum.Game_sequence.Start:
                break;
            case Utill_Enum.Game_sequence.Stop:
                break;
            default:
                break;
        }
    }

    public void Init_WaveUi()
    {
        for (int i = 0; i < CountDownText.Length; i++)
        {
            CountDownText[i].text = startDelay.ToString();
        }
        
        UpdateWaveUI();
    }


    private void UpdateWaveUI()
    {
        if (CurrnetEnemyCount >= 100)
        {
            GameEventSystem.GameOverEvent();
            return;
        }

        StringBuilder str = new StringBuilder();

        // 현재 웨이브 표시
        str.Append("WAVE ").Append(currentWaveIndex + 1);
        WaveText.text = str.ToString();
        str.Clear();

        // 몬스터 카운트 표시
        str.Append(CurrnetEnemyCount).Append("/").Append(MaxEnemyCount);
        currentEnemyCount.text = str.ToString();

        // 적 진행 퍼센트바 업데이트
        EnemyPrograssPersentbar.fillAmount = (float)CurrnetEnemyCount / MaxEnemyCount;
    }

    public void SpawnWaveStart()
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        while (currentWaveIndex < waveData.WaveList.Count)
        {
            isWaveActive = true;
            yield return StartCoroutine(StartWave(waveData.WaveList[currentWaveIndex], waveData.WaveList[currentWaveIndex].Wave_index));
            currentWaveIndex++;
            if (currentWaveIndex == 100)
            {
                GameEventSystem.GameOverEvent();
            }
            isWaveActive = false;
        }
    }

    private IEnumerator StartWave(MonsterWaveDataSO.WaveInfo wave, int waveNumber)
    {
        // UI 업데이트
        UpdateWaveUI();
        WaveTime.text = wave.SpawnTime.ToString("F1");


        //젤첫번쨰만 카운트다움함
        if (waveNumber == 1)
        {
            // 카운트다운 시작
            for (int i = 0; i < CountDownTextObj.Length; i++)
            {
                CountDownTextObj[i].gameObject.SetActive(true);
            }


            for (float time = startDelay; time > 0; time -= 1f)
            {
                CountDownText[0].text = time.ToString("F0");
                CountDownText[1].text = time.ToString("F0");
                yield return new WaitForSeconds(1f);
            }

            for (int i = 0; i < CountDownTextObj.Length; i++)
            {
                CountDownTextObj[i].gameObject.SetActive(false);
            }
        }

        

        float elapsedTime = 0f;
        bool countdownStarted = false; // 카운트다운 중복 실행 방지

        while (elapsedTime < wave.SpawnTime)
        {
            float remainingTime = wave.SpawnTime - elapsedTime;

            // 카운트다운 시작
            if (!countdownStarted && Mathf.Approximately(remainingTime, 5f))
            {
                countdownStarted = true;
                StartCoroutine(CountdownRoutine());
            }

            // 10, 20, 30 웨이브에서는 보스 몬스터 1마리만 소환
            if (waveNumber % 10 == 0 && CurrnetEnemyCount < MaxEnemyCount)
            {
                //보스 팝업 등장
                ActiveBoss(wave.monsterName , wave.Wave_index , wave.SpawnTime);

                playerSpawner.SpawnMonster(wave.monsterName, playerPath);
                aiSpawner.SpawnMonster(wave.monsterName, aiPath);
                CurrnetEnemyCount += 2;
                UpdateWaveUI();
                break; // 보스는 1마리만 소환하고 종료
            }
            else if (CurrnetEnemyCount < MaxEnemyCount) // 일반 몬스터 소환
            {
                playerSpawner.SpawnMonster(wave.monsterName, playerPath);
                aiSpawner.SpawnMonster(wave.monsterName, aiPath);
                CurrnetEnemyCount += 2;
                UpdateWaveUI();
            }

            yield return new WaitForSeconds(spawnInterval);
            elapsedTime += spawnInterval;
            WaveTime.text = (wave.SpawnTime - elapsedTime).ToString("F1");
            if (waveNumber % 10 == 0 && wave.SpawnTime - elapsedTime <= 0.0f)
            {
                GameEventSystem.GameOverEvent();
            }
        }

        // ✅ 웨이브 종료 시 카운트다운 UI 숨김
        for (int i = 0; i < CountDownTextObj.Length; i++)
        {
            CountDownTextObj[i].gameObject.SetActive(false);
        }
    }

    // ✅ **5, 4, 3, 2, 1초 카운트다운 실행하는 코루틴**
    private IEnumerator CountdownRoutine()
    {
        for (int time = 5; time > 0; time--)
        {
            CountDownTextObj[0].gameObject.SetActive(true);
            CountDownTextObj[1].gameObject.SetActive(true);
            CountDownText[0].text = time.ToString();
            CountDownText[1].text = time.ToString();
            yield return new WaitForSeconds(1f);
        }

        // ✅ 0초 이후에는 카운트다운 UI 끄기
        CountDownTextObj[0].gameObject.SetActive(false);
        CountDownTextObj[1].gameObject.SetActive(false);
    }

    public void OnMonsterDeath()
    {
        CurrnetEnemyCount = Mathf.Max(0, CurrnetEnemyCount - 1);
        UpdateWaveUI();
    }
}
