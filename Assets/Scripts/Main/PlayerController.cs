using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement Settings")]
    private float Move;
    public float Direction = 1;
    [SerializeField] float Speed = 2f;

    public Rigidbody2D rb { get; private set; }
    private Animator animator;
    private GhostingEffect ghostingEffect;
    private StateMachine Machine;
    [SerializeField] GameObject cameraFollowObject;
    private CameraFollow Camera;
    private BoxCollider2D Box;
    private ComboCharacter comboCharacter;
    private HealthSystem Health;

    [Header("Jump Settings")]
    private bool bOnGround = true;
    private bool bIsJumping = false;
    [SerializeField] float JumpForce = 5f;
    [SerializeField] float AirControl = 0.8f;
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

    [Header("Wall Sliding Setting")]
    private bool bIsWallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;
    [SerializeField] Transform wallCheck;
    [SerializeField] LayerMask wallCheckLayer;

    [Header("Wall Jumping Setting")]
    [SerializeField] float WallJumpingDuration = 0.2f;
    [SerializeField] Vector2 wallJumpingForce = new Vector2(8f, 16f);
    private float wallJumpingCounter;

    [Header("Gravity Settings")]
    [SerializeField] float FallMultiplier = 2.5f;
    [SerializeField] float MaxJumpTime = 0.2f;
    private float JumpTimeCounter;

    public bool OnGround { get { return bOnGround; } }
    public bool IsWallSliding { get { return bIsWallSliding; } }
    public bool IsDashing { get { return bIsDashing; } }

    [Header("Input")]
    [SerializeField] InputAction MoveAction;
    [SerializeField] InputAction Jump;
    [SerializeField] InputAction DashAction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ghostingEffect = GetComponent<GhostingEffect>();
        Machine = GetComponent<StateMachine>();
        Camera = cameraFollowObject.GetComponent<CameraFollow>();
        comboCharacter = GetComponent<ComboCharacter>();
        Box = GetComponent<BoxCollider2D>();
        Health = GetComponent<HealthSystem>();

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
        {
            coyoteTimeCounter = coyoteTime;
            if(bIsWallSliding)
            {
                bIsWallSliding = false;
                animator.SetBool("IsWallSliding", bIsWallSliding);
            }
        }
        else
        {
            WallSlide();
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (!bIsWallSliding)
        {
            if (Jump.WasPressedThisFrame() && coyoteTimeCounter > 0 && bOnGround && !bIsJumping)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce);
                coyoteTimeCounter = 0;
                bIsJumping = true;
                JumpTimeCounter = MaxJumpTime;
            }
            if (Jump.IsPressed() && bIsJumping)
            {
                if (JumpTimeCounter > 0)
                {
                    JumpTimeCounter -= Time.deltaTime;
                }
                else
                    bIsJumping = false;
            }
            if (!Jump.IsPressed())
                bIsJumping = false;
        }
        else
        {
            if(!bIsJumping)
                WallJumping();
        }

        if(DashAction.WasPressedThisFrame() && bCanDash && !bIsWallSliding)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (bIsDashing)
            return;
        if (!bIsWallSliding)
            HorizontalMovement();

        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);

        bOnGround = Physics2D.Raycast(rb.position, Vector2.down, LenghtGroundCheck, WhatIsGround);
        animator.SetBool("OnGround", bOnGround);

        //Jump
        if (!bIsDashing)
        {
            if ((rb.velocity.y < 0 || (rb.velocity.y > 0 && !bIsJumping)) && !bIsWallSliding) // Falling
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
        if (!comboCharacter.combatMode)
        {
            if (bOnGround)
                rb.velocity = new Vector2(Move * Speed, rb.velocity.y);
            else
                rb.velocity = new Vector2(Move * Speed * AirControl, rb.velocity.y);
        }
        else
            rb.velocity = new Vector2(0, 0);
    }
    
    private void FlipSprite()
    {
        if (Move > 0 && Direction < 0 || Move < 0 && Direction > 0)
        {
            //transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            if (Direction < 0)
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                Camera.CallTurn();
            }
            else if (Direction > 0)
            {
                Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
                transform.rotation = Quaternion.Euler(rotator);
                Camera.CallTurn();
            }
        }
    }
    
    private IEnumerator Dash()
    {
        bCanDash = false;
        bIsDashing = true;
        animator.SetBool("IsDashing", true);
        ghostingEffect.enableGhost = true;
        Health.bCanBeDamage = false;
        float OriginalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(Direction * DashPower, 0f);
        yield return new WaitForSeconds(DashingTime);
        Health.bCanBeDamage = true;
        rb.gravityScale = OriginalGravity;
        bIsDashing = false;
        animator.SetBool("IsDashing", false);
        ghostingEffect.enableGhost = false;
        yield return new WaitForSeconds(DashCoolDown);
        bCanDash = true;
    }

    private bool IsWall()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallCheckLayer);
    }

    private void WallSlide()
    {
        if (rb.velocity.y <= 0)
        {
            if (IsWall() && !bOnGround && Move != 0f && !bIsDashing && !bIsJumping)
            {
                bIsWallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                bIsWallSliding = false;
            }
            animator.SetBool("IsWallSliding", bIsWallSliding);
        }
    }

    private void WallJumping()
    {
        if (wallJumpingCounter > 0)
        {
            wallJumpingCounter -= Time.deltaTime;
            if (Jump.WasPressedThisFrame() && wallJumpingCounter > 0)
            {
                bIsJumping = true;
                rb.velocity = new Vector2(wallJumpingForce.x * Direction * -1, wallJumpingForce.y);
                wallJumpingCounter = 0;
            }
        }
        else
        {
            bIsJumping = false;
            wallJumpingCounter = WallJumpingDuration;
        }
    }

    private IEnumerator TakeDamage()
    {
        Health.bCanBeDamage = false;
        yield return new WaitForSeconds(2f);
        Health.bCanBeDamage = true;
    }

    private IEnumerator Dead()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void OnTakeDamage()
    {
        StartCoroutine(TakeDamage());
    }

    public void OnDead()
    {
        StartCoroutine (Dead());
    }

    private void Attack()
    {

    }
}
