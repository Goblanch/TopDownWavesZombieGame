using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;

    void Awake()
    {
        stateMachine.ConfigureStateMachine(this);
    }
}