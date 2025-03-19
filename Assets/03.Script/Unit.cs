using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] bool ai;
    public bool Ai
    {
        get { return ai; }
        set { ai = value; }
    }
    public FieldSlot fieldSlot;//유저가 있는 슬롯 크로스 체크 용도...

    [SerializeField] Animator anim;
    private List<Enemy> targets = new List<Enemy>(); // 공격 대상 리스트
    private Coroutine attackCoroutine; // 공격 루틴 저장
    [SerializeField] UnitSO _unitso;
    public UnitSO UnitSo
    {
        get { return _unitso; }
        set { _unitso = value; }
    }



    [SerializeField] private bool isAttacking = false; //현재 공격중인지 체크


    public void Init(UnitSO _unitso) // 데이터 초기화
    {
        UnitSo = _unitso;
    }


    private void OnDisable()
    {
        StopCoroutine(DetectEnemies());
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);

        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
        
    }

    private IEnumerator DetectEnemies()
    {
        while (true)
        {
            targets.Clear();
            //캐릭터가아닌 소환된 슬롯의 위치를 기준으로
            Collider[] hitColliders = Physics.OverlapSphere(fieldSlot.transform.position, UnitSo.AttackRange, LayerMask.GetMask(Utill_Constains.Enemy));

            if (hitColliders.Length > 0) //공격할 적이있읅 경우
            {
                foreach (Collider collider in hitColliders)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        targets.Add(enemy);
                    }
                }

                // 공격 대상이 있을 경우 공격 실행
                if (targets.Count > 0 && attackCoroutine == null)
                {
                    attackCoroutine = StartCoroutine(Attack());
                }
            }
            else
            {
                isAttacking = false;
                anim.SetBool(Utill_Constains.IsAttack, isAttacking);
            }
            yield return new WaitForSeconds(0.1f); // 0.5초마다 적 탐색
        }
    }

    internal void Setting()
    {
        StartCoroutine(DetectEnemies()); // 적 감지 시작
    }

    private IEnumerator Attack()
    {
        while (targets.Count > 0)
        {
            int attackCount = UnitSo.AttackCount; // 등급에 따라 공격 가능한 대상 수
            for (int i = 0; i < Mathf.Min(attackCount, targets.Count); i++)
            {
                if (targets[i] != null)
                {
                    isAttacking = true;
                    anim.SetBool(Utill_Constains.IsAttack, isAttacking);
                    targets[i].TakeDamage(UnitSo.Attack , Ai);
                }
            }
            yield return new WaitForSeconds(UnitSo.AttackSpeed); // 공격 속도에 따라 대기
        }
        attackCoroutine = null;
    }

    public override bool Equals(object obj)
    {
        if (obj is Unit otherUnit)
        {
            return this.name == otherUnit.name; // 이름이 같으면 같은 유닛으로 간주
        }
        return false;
    }

    public override int GetHashCode()
    {
        return gameObject.name.GetHashCode();
    }

    // 공격 범위 표시용 Gizmos
    private void OnDrawGizmos()
    {
        if (UnitSo != null)
        {
            // 공격 범위 Gizmos 그리기
            Gizmos.color = Color.black; // 검은색으로 설정
            Gizmos.DrawWireSphere(transform.position, UnitSo.AttackRange); // 유닛의 위치를 기준으로 공격 범위 원 그리기
        }
    }
}
