using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboCharacter : MonoBehaviour
{
    private StateMachine meleeStateMachine;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] InputAction lightComboButton;

    private void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
        lightComboButton.Enable();
    }

    private void Update()
    {
        if (meleeStateMachine.currentState.GetType() == typeof(IdleCombatState))
        {
            if (lightComboButton.WasPressedThisFrame())
            {
                meleeStateMachine.SetNextState(new OnGroundLightEntryState(lightComboButton));
            }
        }
    }
}
