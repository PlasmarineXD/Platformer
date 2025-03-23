using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.InputSystem;

public class OnGroundLightEntryState : MeleeBaseState
{
    public OnGroundLightEntryState(InputAction action) : base(action)
    {
        TransitionStateInput = action;
    }
    public override void OnEnter(StateMachine machine)
    {
        base.OnEnter(machine);
        attackIndex = 1;
        duration = 0.2f;
        animator.SetTrigger("LightAttack" + attackIndex);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (fixedtime >= duration)
        {
            if (shouldCombo)
                Machine.SetNextState(new OnGroundLightComboState(this.TransitionStateInput));
            else
                Machine.SetNextStateToMain();
        }
    }
}
