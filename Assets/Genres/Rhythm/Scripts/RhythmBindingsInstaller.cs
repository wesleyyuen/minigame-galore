using Zenject;

public class RhythmBindingsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<RhythmInput>().AsSingle();
    }
}