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
    public FieldSlot fieldSlot;//������ �ִ� ���� ũ�ν� üũ �뵵...

    [SerializeField] Animator anim;
    private List<Enemy> targets = new List<Enemy>(); // ���� ��� ����Ʈ
    private Coroutine attackCoroutine; // ���� ��ƾ ����
    [SerializeField] UnitSO _unitso;
    public UnitSO UnitSo
    {
        get { return _unitso; }
        set { _unitso = value; }
    }



    [SerializeField] private bool isAttacking = false; //���� ���������� üũ


    public void Init(UnitSO _unitso) // ������ �ʱ�ȭ
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
            //ĳ���Ͱ��ƴ� ��ȯ�� ������ ��ġ�� ��������
            Collider[] hitColliders = Physics.OverlapSphere(fieldSlot.transform.position, UnitSo.AttackRange, LayerMask.GetMask(Utill_Constains.Enemy));

            if (hitColliders.Length > 0) //������ �����֟� ���
            {
                foreach (Collider collider in hitColliders)
                {
                    Enemy enemy = collider.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        targets.Add(enemy);
                    }
                }

                // ���� ����� ���� ��� ���� ����
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
            yield return new WaitForSeconds(0.1f); // 0.5�ʸ��� �� Ž��
        }
    }

    internal void Setting()
    {
        StartCoroutine(DetectEnemies()); // �� ���� ����
    }

    private IEnumerator Attack()
    {
        while (targets.Count > 0)
        {
            int attackCount = UnitSo.AttackCount; // ��޿� ���� ���� ������ ��� ��
            for (int i = 0; i < Mathf.Min(attackCount, targets.Count); i++)
            {
                if (targets[i] != null)
                {
                    isAttacking = true;
                    anim.SetBool(Utill_Constains.IsAttack, isAttacking);
                    targets[i].TakeDamage(UnitSo.Attack , Ai);
                }
            }
            yield return new WaitForSeconds(UnitSo.AttackSpeed); // ���� �ӵ��� ���� ���
        }
        attackCoroutine = null;
    }

    public override bool Equals(object obj)
    {
        if (obj is Unit otherUnit)
        {
            return this.name == otherUnit.name; // �̸��� ������ ���� �������� ����
        }
        return false;
    }

    public override int GetHashCode()
    {
        return gameObject.name.GetHashCode();
    }

    // ���� ���� ǥ�ÿ� Gizmos
    private void OnDrawGizmos()
    {
        if (UnitSo != null)
        {
            // ���� ���� Gizmos �׸���
            Gizmos.color = Color.black; // ���������� ����
            Gizmos.DrawWireSphere(transform.position, UnitSo.AttackRange); // ������ ��ġ�� �������� ���� ���� �� �׸���
        }
    }
}
