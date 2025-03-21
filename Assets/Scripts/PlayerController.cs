using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System.Runtime.CompilerServices;

public class PlayerController : MonoBehaviour
{
    //Movement
    [Header("Movement")]
    private Rigidbody2D rb;
    private float Move;
    private float Direction;
    [SerializeField] float Speed = 2f;

    [Header("Jump")]
    private bool bOnGround = true;
    [SerializeField] float JumpForce = 5f;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] float LenghtGroundCheck = 0.5f;
    [SerializeField] float GroundDrag = 1f;

    [SerializeField] float coyoteTime = 0.2f;
    [SerializeField] float coyoteTimeCounter;

    [Header("Input")]
    [SerializeField] InputAction MoveAction;
    [SerializeField] InputAction Jump;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        MoveAction.Enable();
        Jump.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Move = MoveAction.ReadValue<float>();
        if(!Mathf.Approximately(Move, 0f))
        {
            Direction = Move;
        }

        if (bOnGround)
        {
            coyoteTimeCounter = coyoteTime;
            rb.drag = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            rb.drag = GroundDrag;
        }

        if (Jump.WasPressedThisFrame() && coyoteTimeCounter > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
            coyoteTimeCounter = 0;
        }
    }

    private void FixedUpdate()
    {
        HorizontalMovement();
        bOnGround = Physics2D.Raycast(rb.position, Vector2.down, LenghtGroundCheck, WhatIsGround);
    }

    void HorizontalMovement()
    {
        rb.velocity = new Vector2(Move * Speed, rb.velocity.y);
    }
}
