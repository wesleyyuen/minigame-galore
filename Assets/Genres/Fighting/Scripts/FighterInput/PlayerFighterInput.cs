using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerFighterInput", menuName = "ScriptableObjects/Fighting/FighterInput/PlayerFighterInput")]
public class PlayerFighterInput : FighterInput, InputMaster.IFightingActions
{
    private InputMaster _inputMaster;
    public event Action Event_Pause;

    private void OnEnable()
    {
        if (_inputMaster == null) {
            _inputMaster = new InputMaster();

            _inputMaster.Fighting.SetCallbacks(this);
        }

        _inputMaster.Fighting.Enable();
    }

    private void OnDisable()
    {
        _inputMaster.Fighting.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _inputMaster.Fighting.Disable();
        MainMenu.ToMainMenu();
    }

    public void OnPlayerMovement(InputAction.CallbackContext context)
    {
    }

    public override Vector2 GetDirectionalInputVector()
    {
        return _inputMaster.Fighting.PlayerMovement.ReadValue<Vector2>();
    }

    public override event Action Event_Jump, Event_JumpCanceled;
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Event_Jump?.Invoke();
        else if (context.canceled) Event_JumpCanceled?.Invoke();
    }

    public override bool HasJumpInput()
    {
        return _inputMaster.Fighting.Jump.ReadValue<float>() > 0.5f;
    }
}
