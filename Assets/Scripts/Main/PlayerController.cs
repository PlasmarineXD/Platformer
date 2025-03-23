using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement Settings")]
    private float Move;
    private float Direction = 1;
    [SerializeField] float Speed = 2f;

    public Rigidbody2D rb { get; private set; }
    private Animator animator;


    [Header("Jump Settings")]
    private bool bOnGround = true;
    private bool bIsJumping = false;
    [SerializeField] float JumpForce = 5f;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] float LenghtGroundCheck = 0.5f;

    [SerializeField] float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Dash Setting")]
    private bool bCanDash = true;
    private bool bIsDashing;
    [SerializeField] float DashPower = 24f;
    [SerializeField] float DashingTime = 0.2f;
    [SerializeField] float DashCoolDown = 1f;
    [SerializeField] TrailRenderer TrailRenderer;

    [Header("Gravity Settings")]
    [SerializeField] float FallMultiplier = 2.5f;
    [SerializeField] float MaxJumpTime = 0.2f;
    private float JumpTimeCounter;

    [Header("Input")]
    [SerializeField] InputAction MoveAction;
    [SerializeField] InputAction Jump;
    [SerializeField] InputAction DashAction;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        MoveAction.Enable();
        Jump.Enable();
        DashAction.Enable();
    }

    private void Update()
    {
        if (bIsDashing)
            return;

        //Horizontal Movement
        Move = MoveAction.ReadValue<float>();
        FlipSprite();
        if (!Mathf.Approximately(Move, 0f))
            Direction = Move;

        //Jumping
        if (bOnGround)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;


        if (Jump.WasPressedThisFrame() && coyoteTimeCounter > 0 && bOnGround && !bIsJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            coyoteTimeCounter = 0;
            bIsJumping = true;
            JumpTimeCounter = MaxJumpTime;
        }
        if (Jump.IsPressed() && bIsJumping)
        {
            if(JumpTimeCounter > 0)
            {
                JumpTimeCounter -= Time.deltaTime;
            }
            else
                bIsJumping = false;
        }
        if (!Jump.IsPressed())
            bIsJumping = false;

        if(DashAction.WasPressedThisFrame() && bCanDash)
        {
            StartCoroutine(Dash());
        }

    }

    private void FixedUpdate()
    {
        if (bIsDashing)
            return;

        HorizontalMovement();
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        bOnGround = Physics2D.Raycast(rb.position, Vector2.down, LenghtGroundCheck, WhatIsGround);
        animator.SetBool("OnGround", bOnGround);
        //Debug.DrawLine(rb.position, Vector2.down * LenghtGroundCheck, Color.red);


        //Jump
        if (!bIsDashing)
        {
            if (rb.velocity.y < 0 || (rb.velocity.y > 0 && !bIsJumping)) // Falling
            {
                rb.gravityScale = FallMultiplier;
            }
            else // Normal gravity
            {
                rb.gravityScale = 1f;
            }
        }
    }

    private void HorizontalMovement()
    {
        rb.velocity = new Vector2(Move * Speed, rb.velocity.y);
    }
    
    private void FlipSprite()
    {
        if (Move > 0 && Direction < 0 || Move < 0 && Direction > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
    
    private IEnumerator Dash()
    {
        bCanDash = false;
        bIsDashing = true;
        animator.SetBool("IsDashing", true);
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Direction * DashPower, 0f);
        TrailRenderer.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        TrailRenderer.emitting = false;
        rb.gravityScale = OriginalGravity;
        bIsDashing = false;
        animator.SetBool("IsDashing", false);
        yield return new WaitForSeconds(DashCoolDown);
        bCanDash = true;
    }
}
