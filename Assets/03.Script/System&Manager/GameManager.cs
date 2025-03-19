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
        //���ӿ��� �ڷ�ƾ
        StartCoroutine(GameOverCorutine());
    }

    IEnumerator GameOverCorutine()
    {
        // ���� �Ͻ� ����
        Time.timeScale = 0f;

        // 0.5�� ��� (������ �Ͻ� ������ ���� ���)
        yield return new WaitForSecondsRealtime(0.5f);

        // ���� ���� ������Ʈ Ȱ��ȭ
        gameoverobj.SetActive(true);

        // 1.5�� ��� (���� ���� ȭ���� ��� �����ֱ�)
        yield return new WaitForSecondsRealtime(1.5f);

        // ���� ó��
#if UNITY_EDITOR
        // �����Ϳ��� ���� ���� ������ ����
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����� ���ӿ��� ���ø����̼� ����
        Application.Quit();
#endif
    }


    private IEnumerator GameSequenceRoutine()
    {
        while (Game_sequenceindex < 4)
        {
            if (Game_sequenceindex == 0)
            {
                yield return new WaitForSeconds(2f); // 2�� ���
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.DataLoad);
            }
            else if (Game_sequenceindex == 1)
            {
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.Deley);
            }
            else if (Game_sequenceindex == 2)
            {
                yield return new WaitForSeconds(3f); // 3�� ���
                GameEventSystem.GameGameSequenceEvent(Utill_Enum.Game_sequence.Start);
            }

            Game_sequenceindex++;
            yield return null; // ���� �����ӱ��� ���
        }
    }
}