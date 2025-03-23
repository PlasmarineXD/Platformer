using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeBaseState : State
{
    public float duration;

    protected Animator animator;
    protected bool shouldCombo = false;
    protected float attackIndex;

    private float AttackPressedTimer = 0;

    public InputAction TransitionStateInput;
    public MeleeBaseState(InputAction action)
    {
        TransitionStateInput = action;
    }

    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        animator = GetComponent<Animator>();
        TransitionStateInput.Enable();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        AttackPressedTimer -= Time.deltaTime;
        if (TransitionStateInput.WasPressedThisFrame())
        {
            AttackPressedTimer = duration;
        }
        if (AttackPressedTimer > 0)
        {
            shouldCombo = true;
        }
        else 
        {
            shouldCombo = false;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
