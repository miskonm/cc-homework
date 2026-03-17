using Zenject;

namespace Client.Game.Snake
{
    public class SnakeControllerInstaller : Installer<SnakeControllerInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind<ITickable>()
                .To<SnakeController>()
                .AsSingle()
                .NonLazy();
        }
    }
}