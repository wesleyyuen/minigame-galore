using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnBasedRPGInput : InputMaster.ITurnBasedRPGActions
{
    private InputMaster _inputMaster;

    public TurnBasedRPGInput()
    {
        if (_inputMaster == null) {
            _inputMaster = new InputMaster();

            _inputMaster.TurnBasedRPG.SetCallbacks(this);

            _inputMaster.TurnBasedRPG.Enable();
        }
    }

    ~TurnBasedRPGInput()
    {
        _inputMaster.TurnBasedRPG.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _inputMaster.TurnBasedRPG.Disable();
        MainMenu.ToMainMenu();
    }

    public event Action Event_Back;
    public void OnBack(InputAction.CallbackContext context)
    {
        if (context.started) Event_Back?.Invoke();
    }

    public void OnPlayerMovement(InputAction.CallbackContext context)
    {
    }

    public Vector2 GetDirectionalInputVector()
    {
        return _inputMaster.TurnBasedRPG.PlayerMovement.ReadValue<Vector2>();
    }

    public event Action Event_PlayerInteract;
    public void OnPlayerInteract(InputAction.CallbackContext context)
    {
        if (context.started) Event_PlayerInteract?.Invoke();
    }
}
