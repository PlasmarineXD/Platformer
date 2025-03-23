using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGroundLightComboState : MeleeBaseState
{
    public OnGroundLightComboState(InputAction action) : base(action)
    {
        TransitionStateInput = action;
    }
    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        attackIndex = 2;
        duration = 0.25f;
        animator.SetTrigger("LightAttack" + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedtime >= duration)
        {
            if (shouldCombo)
                Machine.SetNextState(new OnGroundLightFinishState(this.TransitionStateInput));
            else
                Machine.SetNextStateToMain();
        }
    }
}
