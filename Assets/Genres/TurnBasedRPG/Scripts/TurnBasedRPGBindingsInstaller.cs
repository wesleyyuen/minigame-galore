using Zenject;
using TurnBasedRPG.Input;

namespace TurnBasedRPG
{
    public class TurnBasedRPGBindingsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.DeclareSignal<PlayerMovedSignal>();
            Container.DeclareSignal<PlayerDetectedSignal>();

            Container.BindInterfacesAndSelfTo<TurnBasedRPGInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoundController>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleStateMachine>().AsSingle();

            // TODO: setup settings configuration to control how to load
            if (true)
            {
                Container.BindInterfacesAndSelfTo<PokemonSpeciesSOLoader>().AsSingle();
            }
            // else
            // {
            //     Container.BindInterfacesAndSelfTo<PokemonSpeciesJSONLoader>().AsSingle();
            // }
            Container.BindInterfacesAndSelfTo<PokemonSpeciesManifest>().AsSingle();
            Container.BindInterfacesAndSelfTo<WildEncounterController>().AsSingle();
        }
    }
}