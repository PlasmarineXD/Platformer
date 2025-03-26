using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class ComboCharacter : MonoBehaviour
{
    private StateMachine meleeStateMachine;
    private PlayerController playerController;
    public bool combatMode = false;

    [SerializeField] public Collider2D hitbox;
    [SerializeField] InputAction lightComboButton;

    private void Start()
    {
        meleeStateMachine = GetComponent<StateMachine>();
        playerController = GetComponent<PlayerController>();
        lightComboButton.Enable();
    }

    private void LateUpdate()
    {
        if (meleeStateMachine.currentState.GetType() == typeof(IdleCombatState))
            combatMode = false;
        if (lightComboButton.WasPressedThisFrame() && (combatMode == playerController.IsDashing == playerController.IsWallSliding == false) && playerController.OnGround == true)
        {
            combatMode = true;
            meleeStateMachine.SetNextState(new OnGroundLightEntryState(lightComboButton));
        }
    }
}
