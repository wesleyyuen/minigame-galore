using Zenject;

public class TurnBasedRPGBindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TurnBasedRPGInput>().AsSingle();
        Container.Bind<RoundController>().AsSingle();
        // Container.Bind<IPokemonSpeciesLoader>().To<PokemonScpeiesJSONLoader>().AsSingle();
        Container.Bind<IPokemonSpeciesLoader>().To<PokemonSpeciesSOLoader>().AsSingle();
        Container.Bind<PokemonSpeciesManifest>().AsSingle();
        Container.Bind<WildEncounterController>().AsSingle();
    }
}