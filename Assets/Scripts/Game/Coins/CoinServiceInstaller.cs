using System;
using Zenject;

namespace Client.Game.Coins
{
    public class CoinServiceInstaller : Installer<CoinServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container
                .Bind(typeof(ICoinService), typeof(IInitializable), typeof(IDisposable))
                .To<CoinService>()
                .FromSubContainerResolve()
                .ByMethod(InstallService)
                .WithKernel()
                .AsSingle();
        }

        private void InstallService(DiContainer subContainer)
        {
            subContainer.Bind<CoinService>().AsSingle();
            subContainer.Bind<ICoinFactory>().To<CoinFactory>().AsSingle();
            subContainer.Bind<ICoinSpawner>().To<CoinSpawner>().AsSingle();
            subContainer
                .Bind(typeof(IInitializable), typeof(IDisposable))
                .To<CoinCollector>()
                .AsSingle()
                .NonLazy();
        }
    }
}