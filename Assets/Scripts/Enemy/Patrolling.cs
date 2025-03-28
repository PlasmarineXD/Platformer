using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Patrolling : MonoBehaviour
{
    [SerializeField] Transform Left;
    [SerializeField] Transform Right;
    [SerializeField] float Speed;
    [SerializeField] float IdleTime;
    [SerializeField] Transform Actor;

    private Vector3 pRight;
    private Vector3 pLeft;
    private bool bIsPatrolling;
    private float IdleTimeCounter;

    public bool bPatrolling;
    public bool bIsPatrol {  get; private set; }
    public float Direction { get; private set; }
    private void Start()
    {
        pRight = Right.position;
        pLeft = Left.position;
        Direction = 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        if(bPatrolling)
        {
            if (Direction >= 1)
            {
                if (Actor.position.x <= pRight.x)
                {
                    MoveInDirection(1);
                }
                else
                {
                    FlipIdle();
                }
            }
            else
            {
                if (Actor.position.x >= pLeft.x)
                {
                    MoveInDirection(-1);
                }
                else
                {
                    FlipIdle();
                }
            }
        }
    }

    private void FlipIdle()
    {
        if (IdleTimeCounter > 0)
        {
            bIsPatrol = false;
            IdleTimeCounter -= Time.deltaTime;
        }
        else
        {
            Flip();
        }
    }

    public void Flip()
    {
        Direction *= -1;
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    private void MoveInDirection(float Direction)
    {
        bIsPatrol = true;
        IdleTimeCounter = IdleTime;
        Actor.position = new Vector2(Actor.position.x + Speed * Direction * Time.deltaTime, Actor.position.y);
    }
}
