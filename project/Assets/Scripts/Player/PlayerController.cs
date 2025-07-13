using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputListener _inputListener;
    //public StateMachine<PlayerController> stateMachine;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController _cc;
    private Vector2 _moveInput;

    #region ENABLE / DISABLE

    void OnEnable()
    {
        _inputListener.OnMoveEvent          += HandleMoveInput;
        _inputListener.OnShootStartEvent    += HandleShootStart;
        _inputListener.OnShootEndEvent      += HandleShootEnd;
    }

    void OnDisable()
    {
        _inputListener.OnMoveEvent          -= HandleMoveInput;
        _inputListener.OnShootStartEvent    -= HandleShootStart;
        _inputListener.OnShootEndEvent      -= HandleShootEnd;
    }

    #endregion

    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        if (_inputListener == null)
            Debug.LogError("PlayerController: Input Listener is empty", this);

        //stateMachine.ConfigureStateMachine(this);
    }

    void Update()
    {
        ApplyMovement();
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void ApplyMovement()
    {
        Vector3 dir = new Vector3(_moveInput.x, 0f, _moveInput.y);
        if (dir.sqrMagnitude > 1f)
        {
            dir.Normalize();
        }

        _cc.Move(dir * moveSpeed * Time.deltaTime);
    }

    private void HandleShootStart()
    {
        Debug.Log("PlayerController: SHOOT START", this);
    }

    private void HandleShootEnd()
    {
        Debug.Log("PlayerController: SHOOT END", this);
    }
}