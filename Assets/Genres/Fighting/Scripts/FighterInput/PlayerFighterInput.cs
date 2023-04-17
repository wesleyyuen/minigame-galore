using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "PlayerFighterInput", menuName = "ScriptableObjects/Fighting/FighterInput/PlayerFighterInput")]
public class PlayerFighterInput : FighterInput, InputMaster.IFightingActions
{
    private InputMaster _inputMaster;

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

#region Movement
    public void OnPlayerMovement(InputAction.CallbackContext context) {}
    public override Vector2 GetDirectionalInputVector()
    {
        return _inputMaster.Fighting.PlayerMovement.ReadValue<Vector2>();
    }
#endregion

#region Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed) Jump();
        else if (context.canceled) CancelJump();
    }

    public override bool HasJumpInput()
    {
        return _inputMaster.Fighting.Jump.ReadValue<float>() > 0.5f;
    }
#endregion

#region Smash
    public void OnSmash(InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            if (context.performed) Smash();
        }
        else if (context.interaction is SlowTapInteraction slowTap)
        {
            // Canceled will become normal Smash
            if (context.canceled) ChargeSmash(0f);
            else if (context.performed) ChargeSmash((float) context.duration);
        }
    }

    public override bool HasSmashInput()
    {
        return _inputMaster.Fighting.Smash.ReadValue<float>() > 0.5f;
    }
#endregion

#region Block
    public void OnBlock(InputAction.CallbackContext context)
    {
        if (context.performed) Block();
        else if (context.canceled) CancelBlock();
    }

    public override bool HasBlockInput()
    {
        return _inputMaster.Fighting.Block.ReadValue<float>() > 0.5f;
    }
#endregion
}
