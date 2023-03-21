using Zenject;

public class TurnBasedRPGBindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TurnBasedRPGInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoundController>().AsSingle();
        // Container.BindInterfacesAndSelfTo<PokemonScpeiesJSONLoader>().To<PokemonScpeiesJSONLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<PokemonSpeciesSOLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<PokemonSpeciesManifest>().AsSingle();
        Container.BindInterfacesAndSelfTo<WildEncounterController>().AsSingle();
    }
}