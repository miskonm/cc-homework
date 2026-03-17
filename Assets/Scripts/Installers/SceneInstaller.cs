using Client.Game;
using Client.Game.Coins;
using Client.Game.Lose;
using Client.Game.Snake;
using Client.UI;
using Zenject;

namespace Client.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            GameUIPresenterInstaller.Install(Container);
            SnakeControllerInstaller.Install(Container);
            CoinServiceInstaller.Install(Container);
            GameLoseServiceInstaller.Install(Container);
            GameServiceInstaller.Install(Container);
        }
    }
}