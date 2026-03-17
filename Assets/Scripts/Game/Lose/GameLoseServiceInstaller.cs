using System;
using Zenject;

namespace Client.Game.Lose
{
    public class GameLoseServiceInstaller : Installer<GameLoseServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind(typeof(IGameLoseService), typeof(IInitializable), typeof(IDisposable))
                .To<GameLoseService>()
                .AsSingle();
        }
    }
}