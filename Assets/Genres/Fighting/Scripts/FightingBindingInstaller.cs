using Zenject;

public class FightingBindingInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayerFighterInput>().AsSingle();
    }
}
