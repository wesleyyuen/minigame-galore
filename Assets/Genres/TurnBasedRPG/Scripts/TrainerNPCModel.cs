using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace TurnBasedRPG
{
    public class TrainerNPCModel : TrainerModel
    {
        public TrainerNPCAI AI;
        [SerializeField] private List<Pokemon> _pokemons = new List<Pokemon>();
        [SerializeField] private float _sightDistance = 6f;
        private SignalBus _signalBus;

        [Inject]
        public void Init(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        protected override void Start()
        {
            base.Start();

            foreach (var pkmn in _pokemons)
            {
                TrainerInfo.AddCreature(new Pokemon(pkmn.Species, pkmn.Level, pkmn.Name));
            }
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<PlayerMovedSignal>(OnPlayerMoved);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<PlayerMovedSignal>(OnPlayerMoved);
        }

        private void OnPlayerMoved(PlayerMovedSignal signal)
        {
            var hit = Physics2D.Raycast(transform.position + new Vector3(0, -0.5f, 0), -transform.up, _sightDistance);

            if (hit.collider != null && hit.collider.CompareTag("Player"))
            {
                _signalBus.TryFire(new PlayerDetectedSignal(transform));
            }
        }
    }
}