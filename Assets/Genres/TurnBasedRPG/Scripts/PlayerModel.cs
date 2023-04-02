using Zenject;

namespace TurnBasedRPG
{
    public class PlayerModel : TrainerModel
    {
        private PokemonSpeciesManifest _pokemonManifest;

        [Inject]
        public void Init(PokemonSpeciesManifest pokemonManifest)
        {
            _pokemonManifest = pokemonManifest;
        }

        protected override void Awake()
        {
            TrainerInfo = new Trainer(_name, this, true);
        }

        protected override void Start()
        {
            base.Start();
            TrainerInfo.AddCreature(new Pokemon(_pokemonManifest.GetRandomCreatureSpecies(), 6));
        }
    }
}