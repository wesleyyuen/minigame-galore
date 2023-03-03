using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Fighting
{
    public class FighterStateMachine : StateMachine
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed; // TODO: factor out to a scriptable object fightertype data class
        [Header("Jump")]
        [SerializeField] private float _jumpVelocity;
        [SerializeField] private float _fallGravity;
        [SerializeField] private float _lowJumpGravity;
        [SerializeField] private float _jumpBufferTime;
        [SerializeField] private float _coyoteTime;
        [SerializeField] private FighterInput _fighterInput;
        public bool isGrounded {get; private set;}

        private Dictionary<GameState, State> _states = new Dictionary<GameState, State>();
        protected override State GetInitialState() => GetState(GameState.Idle);

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private Vector2 _leftGroundDetectionPoint, _rightGroundDetectionPoint;
        private bool _canJump = true;
        private float _jumpBuffer;
        private float _coyoteTimer;
        private bool _startsJumping;

        [Inject]
        public void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _leftGroundDetectionPoint = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y);
            _rightGroundDetectionPoint = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y);

            // Create all states
            _states.Add(GameState.Idle, new IdleState(this));
        }

        private void OnEnable()
        {
            _fighterInput.Event_Jump += OnJump;
            _fighterInput.Event_JumpCanceled += OnJumpCanceled;
        }

        private void OnDisable()
        {
            _fighterInput.Event_Jump -= OnJump;
            _fighterInput.Event_JumpCanceled -= OnJumpCanceled;
        }

        protected override void Update()
        {
            if (!_canJump) return;

            // Coyote Time - allow late-input of jumps after touching the ground
            if (isGrounded) {
                _coyoteTimer = _coyoteTime;
            } else {
                _coyoteTimer -= Time.deltaTime;
            }

            // Jump Buffering - allow pre-input of jumps before touching the ground
            _jumpBuffer -= Time.deltaTime;

            base.Update();
        }

        protected override void FixedUpdate()
        {
            HandleGroundCollision();
            HandleMovement();
            HandleJump();

            base.FixedUpdate();
        }

        public State GetState(GameState state)
        {
            return _states.FirstOrDefault(kvp => kvp.Key == state).Value;
        }

        private void HandleGroundCollision()
        {
            int layerMask = LayerMask.GetMask("Ground");
            _leftGroundDetectionPoint = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y);
            _rightGroundDetectionPoint = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y);
            RaycastHit2D leftGroundHit = Physics2D.Raycast(_leftGroundDetectionPoint, Vector2.down, 0.5f, layerMask);
            RaycastHit2D rightGroundHit = Physics2D.Raycast(_rightGroundDetectionPoint, Vector2.down, 0.5f, layerMask);
            isGrounded = leftGroundHit || rightGroundHit;
        }

        private void HandleMovement()
        {
            Vector3 input = _fighterInput.GetDirectionalInputVector();
            _rb.velocity = new Vector2(input.x * _movementSpeed, _rb.velocity.y);
        }

        private void OnJump()
        {
            if (_canJump) {
                _jumpBuffer = _jumpBufferTime;
                _startsJumping = true;
            }
        }

        private void OnJumpCanceled()
        {
            _coyoteTimer = 0f;
        }

        private void HandleJump()
        {
            if (!_canJump) return;

            if (_startsJumping && _jumpBuffer > 0f && _coyoteTimer > 0f) {
                _startsJumping = false;
                _jumpBuffer = 0f;
                
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.velocity += Vector2.up * _jumpVelocity;
            }

            if (_rb.velocity.y < 0f) {
                _rb.gravityScale = _fallGravity;
            } else if (_rb.velocity.y > 0f && !_fighterInput.HasJumpInput()) {
                _rb.gravityScale = _lowJumpGravity;
            } else {
                _rb.gravityScale = 1f;
            }
        }

    //   public void StepForward(float dist)
    // {
    //     if (Constant.STOP_WHEN_ATTACK && _coll.onGround && _inputManager.GetDirectionalInputVector().x != 0)
    //         Timing.RunCoroutine(_StepForwardCoroutine(_movementSpeed * 3f, 0.035f).CancelWith(gameObject));
    // }

    // private IEnumerator<float> _StepForwardCoroutine(float speed, float time)
    // {
    //     float timer = 0f;

    //     _isLetRBMove = true;
    //     _animations.EnablePlayerTurning(false);
    //     _rb.velocity = Vector2.zero;

    //     while (timer < time) {
    //         timer += Timing.DeltaTime;
    //         _rb.velocity = (_animations.IsFacingRight() ? Vector2.right : Vector2.left) * speed;
    //         yield return Timing.WaitForOneFrame;
    //     }

    //     _animations.EnablePlayerTurning(true);
    //     _isLetRBMove = false;

    //     _audio.PlayFootstepSFX();
    // }

    // public void ApplyKnockback(Vector2 dir, float force, float time)
    // {
    //     _animations.EnablePlayerTurning(false, time);
    //     LetRigidbodyMoveForSeconds(time == 0 ? 0.1f : time);
    //     Timing.RunCoroutine(Utility._ChangeVariableAfterDelay<float>(e => _rb.drag = e, time == 0 ? 0.1f : time, force * 0.1f, 1f).CancelWith(gameObject));
        
    //     _rb.velocity = Vector2.zero;
    //     _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
    // }
    }

    public enum GameState
    {
        Idle
    }
}