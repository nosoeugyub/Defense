using UnityEngine;

//���� SO
[CreateAssetMenu(fileName = "UserData", menuName = "User/UserData")]
public class UserSOData : ScriptableObject
{
    [SerializeField] private int stage; //���� ��������
    public int Stage { get { return stage; } set { stage = value; } }

    [SerializeReference] private int usegold;//��ȯ�� ���Ǵ� ���
    public int UseGold { get { return usegold; } set { usegold = value; } }

    [SerializeField] private int gold;
    public int Gold { get { return gold; } set { gold = value; } }

    [SerializeField] private int dia;
    public int Dia { get { return dia; } set { dia = value; } }

    [SerializeField] private int currentpopulation; //�����α�
    public int Currentpopulation { get { return currentpopulation; } set { currentpopulation = value; } }

    [SerializeField] private int maxpopulation;//�ƽ� �α�
    public int Maxpopulation { get { return maxpopulation; } }

}
