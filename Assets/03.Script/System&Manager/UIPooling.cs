using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPooling : MonoBehaviour
{
    public static UIPooling Instance { get; private set; }

    public Transform[] SellButtons;
    public Transform[] CombinButton;
    public Transform[] CoinUi;
    public Transform[] DiaUi;
    public Transform[] AttackRange;
    public Transform[] Noticetxt;
    public Transform BossAramText;
    public Transform BossClearText;

    private List<Transform> sellButtonPool = new List<Transform>();
    private List<Transform> combinButtonPool = new List<Transform>();
    private List<Transform> coinUiPool = new List<Transform>();
    private List<Transform> diaUiPool = new List<Transform>();
    private List<Transform> AttackRangePool = new List<Transform>();
    private List<Transform> NoticetxtPool = new List<Transform>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializePool(SellButtons, sellButtonPool);
        InitializePool(CombinButton, combinButtonPool);
        InitializePool(AttackRange, AttackRangePool);
        InitializePool(Noticetxt, NoticetxtPool);
        InitializePool(CoinUi, coinUiPool);
    }


    private void InitializePool(Transform[] source, List<Transform> pool)
    {
        foreach (Transform obj in source)
        {
            obj.gameObject.SetActive(false);
            pool.Add(obj);
        }
    }

    public Transform GetFromPool(Transform[] poolArray)
    {
        foreach (Transform obj in poolArray)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }
        return null; // 사용 가능한 오브젝트가 없으면 null 반환
    }

    public void HideAllButtons()
    {
        foreach (Transform btn in SellButtons)
        {
            btn.gameObject.SetActive(false);
        }
        foreach (Transform btn in CombinButton)
        {
            btn.gameObject.SetActive(false);
        }
        foreach (Transform btn in AttackRange)
        {
            btn.gameObject.SetActive(false);
        }
    }
}
