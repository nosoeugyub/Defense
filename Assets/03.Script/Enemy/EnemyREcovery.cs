using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���� ���³� ü�� ��Ȳ�� ������Ʈ���ִ°�
public class EnemyREcovery : MonoBehaviour
{
    [SerializeField] private Image Hp;


    public void Init_Hpbar()
    {
        Hp.fillAmount = 1;
    }

    public void UpdateHpbar(float damage , float currenthp , float Maxhp)
    {
        // �������� �Ծ��� �� ���� HP�� ���
        currenthp -= damage;

        // HP�� 0���� �������� �ʵ��� ����
        currenthp = Mathf.Max(currenthp, 0);

        // �ִ� HP�� UnitSo�� �ٸ� �������� �����´ٰ� ����
        float maxHp = Maxhp;

        // HP ���� ���
        float hpRatio = currenthp / maxHp;

        // HP Bar�� fillAmount�� ����Ͽ� ������Ʈ
        Hp.fillAmount = hpRatio;
    }
}
