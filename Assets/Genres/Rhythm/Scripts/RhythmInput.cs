using System;
using UnityEngine.InputSystem;
using Rhythm;

public class RhythmInput : InputMaster.IRhythmActions
{
    private InputMaster _inputMaster;
    // public event Action Event_Pause;
    public event Action<Column> Event_Beat;

    public RhythmInput()
    {
        if (_inputMaster == null) {
            _inputMaster = new InputMaster();

            _inputMaster.Rhythm.SetCallbacks(this);

            _inputMaster.Rhythm.Enable();
        }
    }

    ~RhythmInput()
    {
        _inputMaster.Rhythm.Disable();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        _inputMaster.Rhythm.Disable();
        MainMenu.ToMainMenu();
    }

    public void OnLeft(InputAction.CallbackContext context)
    {
        if (context.started) Event_Beat?.Invoke(Column.Left);
    }

    public void OnCenter(InputAction.CallbackContext context)
    {
        if (context.started) Event_Beat?.Invoke(Column.Center);
    }

    public void OnRight(InputAction.CallbackContext context)
    {
        if (context.started) Event_Beat?.Invoke(Column.Right);
    }
}
