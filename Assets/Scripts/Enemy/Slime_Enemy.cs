using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_Enemy : Base_Enemy
{
    [SerializeField] float Damage;
    [SerializeField] float AttackRange;
    [SerializeField] float AttackTime;
    [SerializeField] float ChaseTime;
    [SerializeField] float LineOfSight;
    [SerializeField] float ChaseSpeed;
    [SerializeField] float KnockBack;
    [SerializeField] LayerMask TargetLayer;
    [SerializeField] BoxCollider2D BoxCollider;

    private Patrolling Patrol;

    private Vector3 LastSeenPosition;
    private RaycastHit2D Target;

    private float ChaseTimeCounter;
    private bool bAttacking = false;
    private float AttackTimeCounter;

    protected override void Start()
    {
        base.Start();
        BoxCollider = GetComponent<BoxCollider2D>();
        Patrol = GetComponent<Patrolling>();
    }

    protected override void Update()
    {
        if (bAttacking == false)
        {
            Target = SawEnemy();
            if (Target.collider != null)
            {
                Patrol.bPatrolling = false;
                if (InRange() == false)
                {
                    ChaseTimeCounter = ChaseTime;
                    LastSeenPosition = Target.transform.position;
                    ChaseEnemy(Target);
                }
                else
                {
                    AttackTimeCounter = AttackTime;
                    if (InRange() == true)
                    {
                        Anim.SetTrigger("Attack");
                    }
                    bAttacking = true;
                }
            }
            else
            {
                ChaseTimeCounter -= Time.deltaTime;
                if (ChaseTimeCounter < 0)
                {
                    if (Patrol.bPatrolling == false)
                    {
                        Patrol.bPatrolling = true;
                        Anim.SetFloat("xVelocity", 1);
                    }
                    else
                    {
                        if (Patrol.bIsPatrol == true)
                        {
                            Anim.SetFloat("xVelocity", 1);
                        }
                        else
                        {
                            Anim.SetFloat("xVelocity", 0);
                        }
                    }
                }
                else
                {
                    Patrol.bPatrolling = false;
                    if (Mathf.Approximately(Mathf.Abs(transform.position.x - LastSeenPosition.x), LineOfSight - 2) == false)
                    {
                        ChaseEnemy(Target);
                    }
                }
            }
        }
        else
        {
            Anim.SetFloat("xVelocity", 0);
            if (AttackTimeCounter > 0)
            {
                AttackTimeCounter -= Time.deltaTime;
            }
            else
            {
                if (InRange() == true)
                {
                    Anim.SetTrigger("Attack");
                }
                bAttacking = false;
            }
        }
    }

    private RaycastHit2D SawEnemy()
    {
        return Physics2D.BoxCast(BoxCollider.bounds.center + transform.right * transform.localScale.x * -1,
                                            new Vector3(BoxCollider.bounds.size.x * LineOfSight, BoxCollider.bounds.size.y, BoxCollider.bounds.size.z),
                                            0, Vector2.left, 0, TargetLayer);
    }

    private bool InRange()
    {
        if (Target)
            return Mathf.Abs(transform.position.x - Target.transform.position.x) <= AttackRange;
        else
            return false;
    }

    private void ChaseEnemy(RaycastHit2D Target)
    {
        float direction;
        if (Target.collider != null)
        {
            direction = Mathf.Sign(Target.transform.position.x - transform.position.x);
        }
        else
        {
            direction = Mathf.Sign(LastSeenPosition.x - transform.position.x);
        }
        Anim.SetFloat("xVelocity", 1);
        transform.position = new Vector2(transform.position.x + Time.deltaTime * ChaseSpeed * direction, transform.position.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(BoxCollider.bounds.center + transform.right * transform.localScale.x * -1,
                                            new Vector3(BoxCollider.bounds.size.x * LineOfSight, BoxCollider.bounds.size.y, BoxCollider.bounds.size.z));
    }

    private void Damaging()
    {
        if (InRange())
        {
            Target.transform.GetComponent<HealthSystem>().TakeDamage(Damage);
        }
        else
        {
            Patrol.Flip();
        }
    }
}
