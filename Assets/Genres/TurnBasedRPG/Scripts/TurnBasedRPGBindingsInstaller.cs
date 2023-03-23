using Zenject;
using TurnBasedRPG;

public class TurnBasedRPGBindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        
        Container.DeclareSignal<PlayerDetectedSignal>();
        
        Container.BindInterfacesAndSelfTo<TurnBasedRPGInput>().AsSingle();
        Container.BindInterfacesAndSelfTo<RoundController>().AsSingle();
        // Container.BindInterfacesAndSelfTo<PokemonScpeiesJSONLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<PokemonSpeciesSOLoader>().AsSingle();
        Container.BindInterfacesAndSelfTo<PokemonSpeciesManifest>().AsSingle();
        Container.BindInterfacesAndSelfTo<WildEncounterController>().AsSingle();
    }
}