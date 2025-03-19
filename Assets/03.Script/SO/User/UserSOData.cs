using UnityEngine;

//유저 SO
[CreateAssetMenu(fileName = "UserData", menuName = "User/UserData")]
public class UserSOData : ScriptableObject
{
    [SerializeField] private int stage; //현재 스테이지
    public int Stage { get { return stage; } set { stage = value; } }

    [SerializeReference] private int usegold;//소환에 사용되는 골드
    public int UseGold { get { return usegold; } set { usegold = value; } }

    [SerializeField] private int gold;
    public int Gold { get { return gold; } set { gold = value; } }

    [SerializeField] private int dia;
    public int Dia { get { return dia; } set { dia = value; } }

    [SerializeField] private int currentpopulation; //현재인구
    public int Currentpopulation { get { return currentpopulation; } set { currentpopulation = value; } }

    [SerializeField] private int maxpopulation;//맥스 인구
    public int Maxpopulation { get { return maxpopulation; } }

}
