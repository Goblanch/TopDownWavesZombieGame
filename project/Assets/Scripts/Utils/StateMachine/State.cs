using UnityEngine;

public abstract class State<TController> : MonoBehaviour where TController : MonoBehaviour
{
    protected StateMachine<TController> stateMachine;
    protected TController controller;

    public void Confifure(StateMachine<TController> stateMachine, TController controller)
    {
        this.stateMachine = stateMachine;
        this.controller = controller;
    }

    public abstract void StateInputs();
    public abstract void StateEnter();
    public abstract void StateStep();
    public abstract void StatePhysicsStep();
    public abstract void StateLateStep();
    public abstract void StateExit();
}
