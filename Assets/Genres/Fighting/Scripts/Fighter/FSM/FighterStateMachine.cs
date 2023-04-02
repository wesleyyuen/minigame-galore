using System.Linq;
using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Fighting
{
    public class FighterStateMachine : MonoStateMachine
    {
        [Header("Input")]
        [SerializeField] private FighterInput _fighterInput;
        [Header("Collision")]
        [SerializeField] private Collider2D _collisionBlocker;
        
        [Header("Movement")]
        [SerializeField] private float _movementSpeed; // TODO: factor out to a scriptable object fightertype data class

        [Header("Jump")]
        [SerializeField] private float _jumpVelocity;
        [SerializeField] private float _fallGravity;
        [SerializeField] private float _lowJumpGravity;
        [SerializeField] private float _jumpBufferTime;
        [SerializeField] private float _coyoteTime;
        public bool isGrounded {get; private set;}

        [Header("Health")]
        [SerializeField] private float _maxHealth;
        private float _currentHealth;
        protected override MonoState GetInitialState() => GetState(GameState.Idle.ToString());

        private Rigidbody2D _rb;
        private Collider2D _collider;
        private Animator _animator;
        private HitHandler _hitHandler;
        private Vector2 _leftGroundDetectionPoint, _rightGroundDetectionPoint;
        private bool _canJump = true;
        private float _jumpBuffer;
        private float _coyoteTimer;
        private bool _startsJumping;
        private bool _letRigidbodyMove;

        private FighterInput _input;

        [Inject]
        public void Init()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _animator = GetComponent<Animator>();
            _hitHandler = GetComponentInChildren<HitHandler>();
            _leftGroundDetectionPoint = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y);
            _rightGroundDetectionPoint = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y);

            _currentHealth = _maxHealth;

            // Create all states
            States.Add(GameState.Idle.ToString(), new IdleState(this));
            States.Add(GameState.Attack.ToString(), new AttackState(this, _animator, _hitHandler));
            States.Add(GameState.Block.ToString(), new BlockState(this));

            _input = (FighterInput) ScriptableObject.Instantiate(_fighterInput);
            _input.SetSelf(this);

            Physics2D.IgnoreCollision(_collider, _collisionBlocker, true);
        }

        private void OnEnable()
        {
            _input.Event_Jump += OnJump;
            _input.Event_JumpCanceled += OnJumpCanceled;
            _input.Event_Attack += OnAttack;
            _input.Event_Block += OnBlock;
        }

        private void OnDisable()
        {
            _input.Event_Jump -= OnJump;
            _input.Event_JumpCanceled -= OnJumpCanceled;
            _input.Event_Attack -= OnAttack;
            _input.Event_Block -= OnBlock;
        }

        protected override void Update()
        {
            if (!_canJump) return;

            // Coyote Time - allow late-input of jumps after touching the ground
            if (isGrounded)
            {
                _coyoteTimer = _coyoteTime;
            }
            else
            {
                _coyoteTimer -= Time.deltaTime;
            }

            // Jump Buffering - allow pre-input of jumps before touching the ground
            _jumpBuffer -= Time.deltaTime;

            _input.Update();

            base.Update();
        }

        protected override void FixedUpdate()
        {
            HandleGroundCollision();

            if (_letRigidbodyMove) return;

            HandleMovement();
            HandleJump();

            base.FixedUpdate();
        }

        public void TakeDamage(HitInfo hit)
        {
            _currentHealth = Mathf.Max(0, _currentHealth - hit.Damage);
            ApplyKnockback((Vector2)transform.position - hit.Origin, 25);
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
            var horizontalInput = _input.GetDirectionalInputVector().x;
            _rb.velocity = new Vector2(horizontalInput * _movementSpeed, _rb.velocity.y);

            // Turning
            if (_rb.velocity.x < 0 && transform.localScale.x > 0) transform.localScale = new Vector3(-1, 1, 1);
            else if (_rb.velocity.x > 0 && transform.localScale.x < 0) transform.localScale = new Vector3(1, 1, 1);
        }

        private void OnJump()
        {
            if (_canJump)
            {
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

            if (_startsJumping && _jumpBuffer > 0f && _coyoteTimer > 0f)
            {
                _startsJumping = false;
                _jumpBuffer = 0f;
                
                _rb.velocity = new Vector2(_rb.velocity.x, 0);
                _rb.velocity += Vector2.up * _jumpVelocity;
            }

            if (isGrounded)
            {
                _rb.gravityScale = 1f;
            }
            else if (_rb.velocity.y < 0f)
            {
                _rb.gravityScale = _fallGravity;
            }
            else if (_rb.velocity.y > 0f && !_input.HasJumpInput())
            {
                _rb.gravityScale = _lowJumpGravity;
            }
        }

        private void OnAttack()
        {
            // TODO: check state, or better yet, make attack state check itself
            ChangeState(GameState.Attack.ToString());
        }

        private void OnBlock()
        {
            // Debug.Log("Block");
        }

        private async void ApplyKnockback(Vector2 dir, float force)
        {
            _rb.drag = 5f;
            _letRigidbodyMove = true;

            _rb.velocity = Vector2.zero;
            _rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(0.125f));

            _rb.drag = 1f;
            _letRigidbodyMove = false;
        }
    }

    public enum GameState
    {
        Idle,
        Attack,
        Block
    }
}