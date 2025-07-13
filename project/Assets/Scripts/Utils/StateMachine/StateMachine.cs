using System;
using UnityEngine;

public class StateMachine<TController> : MonoBehaviour where TController : MonoBehaviour
{
    [SerializeField] private State<TController>[] states;
    public State<TController> CurrentState { get; private set; }
    public State<TController> LastState { get; private set; }
    public TController Controller { get; private set; }

    public void ConfigureStateMachine(TController controller)
    {
        Controller = controller;
    }

    #region STATE MACHINE USAGE
    public void Initialize()
    {
        if (states == null || states.Length <= 0)
        {
            Debug.LogError($"State Machone on object {gameObject.transform.parent.name}, is empty");
            return;
        }

        foreach (State<TController> state in states)
        {
            state.Confifure(this, Controller);
        }

        SetState(states[0].GetType());
    }

    public void SetState(Type nextStateType)
    {
        State<TController> nextState = QueryStateByType(nextStateType);
        if (nextState == null || nextState == CurrentState) return;

        CurrentState?.StateExit();
        LastState = CurrentState;
        CurrentState = nextState;
        CurrentState.StateEnter();
    }

    private State<TController> QueryStateByType(Type stateType)
    {
        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].GetType() == stateType) return states[i];
        }

        return null;
    }
    #endregion

    #region STATE MACHINE STEPS
    public void Step()
    {
        if (CurrentState == null) return;

        CurrentState.StateInputs();
        CurrentState.StateStep();
    }

    public void PhysicsStep()
    {
        if (CurrentState == null) return;

        CurrentState.StatePhysicsStep();
    }

    public void LateStep()
    {
        if (CurrentState == null) return;

        CurrentState.StateLateStep();
    }

    #endregion
}
