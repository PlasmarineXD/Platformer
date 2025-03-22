using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainAnimation : MonoBehaviour
{
    private PlayerController playerController;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        animator.SetFloat("xVelocity", Mathf.Abs(playerController.rb.velocity.x));
        animator.SetFloat("yVelocity", playerController.rb.velocity.y);
        animator.SetBool("OnGround", playerController.OnGround);
    }
}
