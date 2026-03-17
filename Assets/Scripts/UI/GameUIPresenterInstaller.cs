using System;
using Zenject;

namespace Client.UI
{
    public class GameUIPresenterInstaller : Installer<GameUIPresenterInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind(typeof(IInitializable), typeof(IDisposable))
                .To<GameUIPresenter>()
                .AsSingle()
                .NonLazy();
        }
    }
}