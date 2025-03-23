using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGroundLightFinishState : MeleeBaseState
{
    public OnGroundLightFinishState(InputAction action) : base(action)
    {
        TransitionStateInput = action;
    }
    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        attackIndex = 3;
        duration = 0.5f;
        animator.SetTrigger("LightAttack" + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedtime >= duration)
        {
            Machine.SetNextStateToMain();
        }
    }
}
