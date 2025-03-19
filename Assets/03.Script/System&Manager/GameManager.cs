using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int Game_sequenceindex;
    public Canvas canvas;

    public static GameManager instance = null;

    public GameObject gameoverobj;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Game_sequenceindex = 0;
        StartCoroutine(GameSequenceRoutine());
        GameEventSystem.GameOver_Event += GameOver;
    }


    public void GameOver()
    {
        //게임오버 코루틴
        StartCoroutine(GameOverCorutine());
    }

    IEnumerator GameOverCorutine()
    {
        // 게임 일시 정지
        Time.timeScale = 0f;

        // 0.5초 대기 (게임이 일시 정지된 동안 대기)
        yield return new WaitForSecondsRealtime(0.5f);

        // 게임 오버 오브젝트 활성화
        gameoverobj.SetActive(true);

        // 1.5초 대기 (게임 오버 화면을 잠시 보여주기)
        yield return new WaitForSecondsRealtime(1.5f);

        // 종료 처리
#if UNITY_EDITOR
        // 에디터에서 실행 중인 게임을 종료
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임에서 애플리케이션 종료
        Application.Quit();
#endif
    }


    private IEnumerator GameSequenceRoutine()
    {
        while (Game_sequenceindex < 4)
        {
            if (Game_sequenceindex == 0)
            {
                yield return new WaitForSeconds(2f); // 2초 대기
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.DataLoad);
            }
            else if (Game_sequenceindex == 1)
            {
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.Deley);
            }
            else if (Game_sequenceindex == 2)
            {
                yield return new WaitForSeconds(3f); // 3초 대기
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.Start);
            }

            Game_sequenceindex++;
            yield return null; // 다음 프레임까지 대기
        }
    }
}