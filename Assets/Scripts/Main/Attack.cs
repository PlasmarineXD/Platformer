using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] Transform AttackPoint;
    [SerializeField] float AttackRange;
    [SerializeField] LayerMask AttackTarget;
    [SerializeField] float Damage;

    public void DealDamage()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(AttackPoint.position, AttackRange, transform.right, 0f, AttackTarget);
        for(int i = 0; i < hits.Length; i++)
        {
            HealthSystem Target = hits[i].transform.GetComponent<HealthSystem>();
            if(Target != null)
            {
                Target.TakeDamage(Damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }
}
