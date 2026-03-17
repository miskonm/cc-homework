using System;
using Zenject;

namespace Client.Game
{
    public class GameServiceInstaller : Installer<GameServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind(typeof(IGameService), typeof(IInitializable), typeof(IDisposable))
                .To<GameService>()
                .AsSingle();
        }
    }
}