using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputListener", menuName = "TDZG/InputListener")]
public class InputListener : ScriptableObject, PlayerInputActions.IGameActions, PlayerInputActions.IUIActions
{
    public enum GameModes
    {
        Game, UI
    }

    #region ACTIONS 

    public Action<Vector2> OnMoveEvent;
    public Action OnShootStartEvent;
    public Action OnShootEndEvent;
    public Action OnPauseEvent;
    public Action OnResumeEvent;

    #endregion

    private PlayerInputActions _playerInput;

    #region INTERFACES IMPLEMENTATIONS

    void OnEnable()
    {
        if (_playerInput == null)
        {
            _playerInput = new PlayerInputActions();

            _playerInput.Game.SetCallbacks(this);
            _playerInput.UI.SetCallbacks(this);

            ChangeGameMode(GameModes.Game);
        }
    }

    void OnDisable()
    {
        _playerInput.Game.Disable();
        _playerInput.UI.Disable();
    }

    public void ChangeGameMode(GameModes gMode)
    {
        switch (gMode)
        {
            case GameModes.Game:
                _playerInput.Game.Enable();
                _playerInput.UI.Disable();
                break;
            case GameModes.UI:
                _playerInput.UI.Enable();
                _playerInput.Game.Disable();
                break;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 dir = context.ReadValue<Vector2>();
            OnMoveEvent?.Invoke(dir);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started || context.performed)
        {
            OnShootStartEvent?.Invoke();
        }
        else if (context.canceled)
        {
            OnShootEndEvent?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPauseEvent?.Invoke();
            ChangeGameMode(GameModes.UI);
        }
    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnResumeEvent?.Invoke();
            ChangeGameMode(GameModes.Game);
        }
    }

    #endregion
}
