using UnityEngine;
using Zenject;
using Cysharp.Threading.Tasks;
using TurnBasedRPG.Input;

namespace TurnBasedRPG
{
    public sealed class OverworldState : MonoState
    {
        private GameStateMachine _fsm;
        private SignalBus _signalBus;
        private TurnBasedRPGInput _input;
        private WildEncounterController _wildEncounterController;
        private Rigidbody2D _rb;
        private bool _isMoving;

        public OverworldState(
            GameStateMachine stateMachine,
            SignalBus signalBus,
            TurnBasedRPGInput input,
            WildEncounterController wildEncounterController) : base(GameState.Overworld.ToString(), stateMachine)
        {
            _fsm = stateMachine;
            _signalBus = signalBus;
            _input = input;
            _wildEncounterController = wildEncounterController;
            _rb = stateMachine.player.gameObject.GetComponent<Rigidbody2D>();
        }

        public override void EnterState(object args = null)
        {
            _signalBus.Subscribe<PlayerDetectedSignal>(OnPlayerDetected);
        }

        private void OnPlayerDetected(PlayerDetectedSignal signal)
        {
            _fsm.ChangeState(GameState.PlayerDetected.ToString(), signal.Detector);
        }

        public override async void UpdateState()
        {
            if (_isMoving) return;

            Vector3 input = _input.GetDirectionalInputVector();

            if (input == Vector3.zero) return;

            // Prioritized x-axis movements
            if (input.x > 0) await Move(Vector2.right);
            else if (input.x < 0) await Move(Vector2.left);
            else if (input.y > 0) await Move(Vector2.up);
            else if (input.y < 0) await Move(Vector2.down);
        }

        private async UniTask Move(Vector2 dir)
        {
            if (!IsWalkable(dir)) return;

            Vector3 from = _rb.position;
            Vector3 to = _rb.position + dir;

            _isMoving = true;

            for (float t = 0f; t < 1f; t += Time.deltaTime / Constants.CHARACTER_MOVEMENT_DURATION)
            {
                _rb.position = Vector3.Lerp(from, to, t);
                await UniTask.NextFrame();
            }

            _rb.position = to;

            _signalBus.TryFire(new PlayerMovedSignal(_rb.transform.position));

            _isMoving = false;

            CheckForWildEncounters(_rb.position);
        }

        private void CheckForWildEncounters(Vector2 pos)
        {
            var encounter = _wildEncounterController.GetWildEncountersAtPostion(pos);
            if (encounter != null)
            {
                _fsm.ChangeState(GameState.WildBattle.ToString(), encounter);
            }
        }

        private bool IsWalkable(Vector2 dir)
        {
            return !Physics2D.Raycast(_rb.position, dir, 0.5f, _fsm.unwalkableLayers);
        }

        public override void ExitState()
        {
            _signalBus.Unsubscribe<PlayerDetectedSignal>(OnPlayerDetected);
        }
    }
}