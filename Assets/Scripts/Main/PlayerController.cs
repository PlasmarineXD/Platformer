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


    [Header("Jump Settings")]
    private bool bOnGround = true;
    private bool bIsJumping = false;
    [SerializeField] float JumpForce = 5f;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] float LenghtGroundCheck = 0.5f;

    [SerializeField] float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    public bool OnGround { get { return bOnGround; } }
    public bool IsJumping { get { return bIsJumping; } }



    [Header("Gravity Settings")]
    [SerializeField] float fallMultiplier = 2.5f;
    [SerializeField] float maxJumpTime = 0.2f;
    private float jumpTimeCounter;

    [Header("Input")]
    [SerializeField] InputAction MoveAction;
    [SerializeField] InputAction Jump;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        MoveAction.Enable();
        Jump.Enable();
    }

    private void Update()
    {
        Debug.Log(rb.velocity.y);
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
            jumpTimeCounter = maxJumpTime;
        }
        if (Jump.IsPressed() && bIsJumping)
        {
            if(jumpTimeCounter > 0)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else
                bIsJumping = false;
        }
        if (!Jump.IsPressed())
            bIsJumping = false;


    }

    private void FixedUpdate()
    {
        HorizontalMovement();

        bOnGround = Physics2D.Raycast(rb.position, Vector2.down, LenghtGroundCheck, WhatIsGround);
        //Debug.DrawLine(rb.position, Vector2.down * LenghtGroundCheck, Color.red);


        //Jump
        if (rb.velocity.y < 0 || (rb.velocity.y > 0 && !bIsJumping)) // Falling
        {
            rb.gravityScale = fallMultiplier;
        }
        else // Normal gravity
        {
            rb.gravityScale = 1f;
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
}
