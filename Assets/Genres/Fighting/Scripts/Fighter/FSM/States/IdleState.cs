using UnityEngine;
using Fighting;

public class IdleState : MonoState
{
    private FighterStateMachine _fsm;
    private float _jumpBuffer;
    private float _coyoteTimer;
    private bool _startsJumping;

    public IdleState(FighterStateMachine stateMachine) : base(FighterState.Idle.ToString(), stateMachine)
    {
        _fsm = stateMachine;
    }
    
    public override void EnterState(object args = null)
    {
        _fsm.Input.Event_Jump += OnJump;
        _fsm.Input.Event_JumpCanceled += OnJumpCanceled;
        _fsm.Input.Event_Smash += OnSmash;
        _fsm.Input.Event_Block += OnBlock;
    }

    public override void UpdateState()
    {
        // Coyote Time - allow late-input of jumps after touching the ground
        if (_fsm.IsGrounded)
        {
            _coyoteTimer = _fsm.Data.CoyoteTime;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }

        // Jump Buffering - allow pre-input of jumps before touching the ground
        _jumpBuffer -= Time.deltaTime;
    }

    public override void FixedUpdateState()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        var horizontalInput = _fsm.Input.GetDirectionalInputVector().x;
        _fsm.Rigidbody.velocity = new Vector2(horizontalInput * _fsm.Data.MovementSpeed, _fsm.Rigidbody.velocity.y);

        // Turning
        if (_fsm.Rigidbody.velocity.x < 0 && _fsm.transform.localScale.x > 0) 
            _fsm.transform.localScale = new Vector3(-Mathf.Abs(_fsm.transform.localScale.x), _fsm.transform.localScale.y, _fsm.transform.localScale.z);
        else if (_fsm.Rigidbody.velocity.x > 0 && _fsm.transform.localScale.x < 0)
            _fsm.transform.localScale = new Vector3(Mathf.Abs(_fsm.transform.localScale.x), _fsm.transform.localScale.y, _fsm.transform.localScale.z);
    }

    private void HandleJump()
    {
        if (_startsJumping && _jumpBuffer > 0f && _coyoteTimer > 0f)
        {
            _startsJumping = false;
            _jumpBuffer = 0f;
            
            _fsm.Rigidbody.velocity = new Vector2(_fsm.Rigidbody.velocity.x, 0);
            _fsm.Rigidbody.velocity += Vector2.up * _fsm.Data.JumpVelocity;
        }

        if (_fsm.IsGrounded || _fsm.Rigidbody.velocity.y < 0f)
        {
            _fsm.Rigidbody.gravityScale = 1f;
        }
        else if (_fsm.Rigidbody.velocity.y > 0f && !_fsm.Input.HasJumpInput())
        {
            _fsm.Rigidbody.gravityScale = _fsm.Data.LowJumpGravity;
        }
    }

    private void OnJump()
    {
        _jumpBuffer = _fsm.Data.JumpBufferTime;
        _startsJumping = true;
    }

    private void OnJumpCanceled()
    {
        _coyoteTimer = 0f;
    }

    private void OnSmash()
    {
        _fsm.ChangeState(FighterState.Charge.ToString());
    }

    private void OnBlock()
    {
        _fsm.ChangeState(FighterState.Block.ToString());
    }

    public override void ExitState()
    {
        _fsm.Input.Event_Jump -= OnJump;
        _fsm.Input.Event_JumpCanceled -= OnJumpCanceled;
        _fsm.Input.Event_Smash -= OnSmash;
        _fsm.Input.Event_Block -= OnBlock;
    }
}
