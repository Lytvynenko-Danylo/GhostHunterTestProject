using Zenject;

public class StartLevelInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ISceneManager>().To<SceneManager>().AsSingle().NonLazy();
    }
}