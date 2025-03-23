using Unity.VisualScripting;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public string customName;

    public State mainStateType;

    public State currentState {  get; private set; }
    private State nextState;

    // Update is called once per frame
    private void Update()
    {
        if(nextState != null)
        {
            SetState(nextState);
        }

        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }
    
    private void SetState(State newstate)
    {
        nextState = null;
        if (currentState != null)
        {
            currentState.OnExit();
        }
        currentState = newstate;
        currentState.OnEnter(this);
    }

    public void SetNextState(State newState)
    {
        if(newState != null)
        {
            nextState = newState;
        }
    }

    private void LateUpdate()
    {
        if(currentState != null)
            currentState.OnLateUpdate();
    }

    private void FixedUpdate()
    {
        if (currentState != null)
            currentState.OnFixedUpdate();
    }

    public void SetNextStateToMain()
    {
        nextState = mainStateType;
    }
    private void Awake()
    {
        SetNextStateToMain();
    }
    private void OnValidate()
    {
        if (mainStateType == null)
        {
            if (customName == "Combat")
            {
                mainStateType = new IdleCombatState();
            }
        }
    }
}
