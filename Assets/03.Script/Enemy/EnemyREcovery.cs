using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//몬스터 상태나 체력 상황을 업데이트해주는곳
public class EnemyREcovery : MonoBehaviour
{
    [SerializeField] private Image Hp;


    public void Init_Hpbar()
    {
        Hp.fillAmount = 1;
    }

    public void UpdateHpbar(float damage , float currenthp , float Maxhp)
    {
        // 데미지를 입었을 때 현재 HP를 계산
        currenthp -= damage;

        // HP가 0보다 적어지지 않도록 보장
        currenthp = Mathf.Max(currenthp, 0);

        // 최대 HP는 UnitSo나 다른 변수에서 가져온다고 가정
        float maxHp = Maxhp;

        // HP 비율 계산
        float hpRatio = currenthp / maxHp;

        // HP Bar의 fillAmount를 계산하여 업데이트
        Hp.fillAmount = hpRatio;
    }
}
