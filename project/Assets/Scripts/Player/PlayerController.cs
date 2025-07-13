using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;

public class PlayerController : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputListener _inputListener;
    [SerializeField] private Camera _mainCamera;
    //public StateMachine<PlayerController> stateMachine;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    private CharacterController _characterController;
    private Vector2 _moveInput;
    private Vector3 _moveDirection;

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
        _characterController = GetComponent<CharacterController>();
        if (_inputListener == null)
            Debug.LogError("PlayerController: Input Listener is empty", this);

        if (_mainCamera == null)
        {
            _mainCamera = Camera.main;
        }

        if (_mainCamera == null)
        {
            Debug.LogError($"{nameof(PlayerController)}: couldn't find Main Camera", this);
        }

        //stateMachine.ConfigureStateMachine(this);
    }

    void Update()
    {
        ApplyMovement();
        RotateTowardsMouse();
    }

    private void HandleMoveInput(Vector2 moveInput)
    {
        _moveInput = moveInput;
    }

    private void ApplyMovement()
    {
        Vector3 localMove = _moveInput.x * transform.right + _moveInput.y * transform.forward;
        
        if (localMove.sqrMagnitude > 1f)
        {
            localMove.Normalize();
        }

        _characterController.Move(localMove * moveSpeed * Time.deltaTime);
    }

    private void RotateTowardsMouse()
    {
        Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 lookDirection = hitPoint - transform.position;
            lookDirection.y = 0f;

            if (lookDirection.sqrMagnitude > 0f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation;
            }
        }
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