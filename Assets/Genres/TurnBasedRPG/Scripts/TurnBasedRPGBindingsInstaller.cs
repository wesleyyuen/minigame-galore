using UnityEngine;
using Zenject;

public class TurnBasedRPGBindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<TurnBasedRPGInput>().AsSingle();
        Container.Bind<RoundController>().AsSingle();
        Container.Bind<PokemonSpeciesManifest>().AsSingle();
        Container.Bind<WildEncounterController>().AsSingle();
    }
}