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
        public bool IsGrounded {get; private set;}
        [Header("Data")]
        [SerializeField] private FighterData _data;
        public FighterData Data => _data;

        private int _stockCount = Constants.STOCK_COUNT;
        private float _currentHealth = 0f;
        protected override MonoState GetInitialState() => GetState(FighterState.Idle.ToString());

        public Rigidbody2D Rigidbody {get; private set;}
        public Animator Animator {get; private set;}
        private Collider2D _collider;
        private HitHandler _hitHandler;
        private bool _letRigidbodyMove;
        private bool _isKnockedOut;

        public FighterInput Input {get; private set;}

        [Inject]
        public void Init()
        {
            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _hitHandler = GetComponentInChildren<HitHandler>();

            // Create all states
            States.Add(FighterState.Idle.ToString(), new IdleState(this));
            States.Add(FighterState.Smash.ToString(), new SmashState(this, _hitHandler));
            States.Add(FighterState.Charge.ToString(), new ChargeState(this));
            States.Add(FighterState.Block.ToString(), new BlockState(this));

            // TODO: better way to handle indepedent input
            Input = (FighterInput) ScriptableObject.Instantiate(_fighterInput);
            Input.SetSelf(this);

            Physics2D.IgnoreCollision(_collider, _collisionBlocker, true);
        }

        protected override void Update()
        {
            if (_isKnockedOut) return;

            Input.Update();

            base.Update();
        }

        protected override void FixedUpdate()
        {
            if (_isKnockedOut) return;

            HandleGroundCollision();

            if (_letRigidbodyMove) return;

            base.FixedUpdate();
        }

        public void TakeDamage(HitInfo hit)
        {
            if (_isKnockedOut) return;

            _currentHealth = _currentHealth + hit.Damage;

            float knockbackForce = hit.Damage * Constants.DAMAGE_KNOCKBACK_MULTIPLIER;
            Vector2 knockbackAngle = (Vector2) Rigidbody.position - hit.Origin;
            knockbackAngle.y = Mathf.Abs(knockbackAngle.x) * 0.5f;

            if (_currentHealth >= Constants.MIN_KNOCKOUT_THRESHOLD)
            {
                float knockoutChance = _currentHealth.MapRange(Constants.MIN_KNOCKOUT_THRESHOLD, Constants.MAX_KNOCKOUT_THRESHOLD, 0f, 1f);
                if (_currentHealth >= Constants.MAX_KNOCKOUT_THRESHOLD || UnityEngine.Random.Range(0f, 1f) <= knockoutChance)
                {
                    // Knocked Out (via offscreen)
                    knockbackForce = Constants.KNOCKOUT_KNOCKBACK_FORCE;
                    ApplyKnockback(knockbackAngle, knockbackForce, 1f);
                }
                else
                {
                    // Increasingly harsh knockbacks
                    knockbackForce *= (1 + knockoutChance);
                    ApplyKnockback(knockbackAngle, knockbackForce);
                }
                Debug.Log($"{gameObject.name} @ {_currentHealth} knocked back with {knockbackForce} with KO chance {knockoutChance}!");
                return;
            }else
            Debug.Log($"{gameObject.name} @ {_currentHealth} knocked back with {knockbackForce}!");
            ApplyKnockback(knockbackAngle, knockbackForce);
        }

        public async void KnockOut()
        {
            _isKnockedOut = true;

            _stockCount--;
            _currentHealth = 0f;

            if (_stockCount > 0)
            {
                Debug.Log($"{gameObject.name} is Knocked out! {_stockCount} remaining");
                await UniTask.Delay(TimeSpan.FromSeconds(2f));
                Respawn();
            }
            else
            {
                Debug.Log($"Game Over! {gameObject.name} is out of stock!");
            }
        }

        private async void Respawn()
        {
            transform.position = Vector3.zero;
            Rigidbody.Sleep();

            await UniTask.Delay(TimeSpan.FromSeconds(2f));

            Rigidbody.WakeUp();
            _isKnockedOut = false;
        }

        private void HandleGroundCollision()
        {
            int layerMask = LayerMask.GetMask("Ground");
            var leftGroundDetectionPoint = new Vector2(_collider.bounds.min.x, _collider.bounds.min.y);
            var rightGroundDetectionPoint = new Vector2(_collider.bounds.max.x, _collider.bounds.min.y);
            RaycastHit2D leftGroundHit = Physics2D.Raycast(leftGroundDetectionPoint, Vector2.down, 0.5f, layerMask);
            RaycastHit2D rightGroundHit = Physics2D.Raycast(rightGroundDetectionPoint, Vector2.down, 0.5f, layerMask);
            IsGrounded = leftGroundHit || rightGroundHit;
        }
        
        private async void ApplyKnockback(Vector2 dir, float force, float duration = 0.125f)
        {
            Rigidbody.drag = 5f;
            _letRigidbodyMove = true;

            Rigidbody.velocity = Vector2.zero;
            Rigidbody.AddForce(dir.normalized * force, ForceMode2D.Impulse);

            await UniTask.Delay(TimeSpan.FromSeconds(duration));

            Rigidbody.drag = 1f;

            await UniTask.WaitUntil(() => IsGrounded);
            _letRigidbodyMove = false;
        }
    }

    public enum FighterState
    {
        Idle,
        Smash,
        Charge,
        Block
    }
}