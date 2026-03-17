using Modules;
using UnityEngine;
using Zenject;

namespace Client.Installers
{
    public class SceneObjectsInstaller : MonoInstaller
    {
        [SerializeField] private Snake _snake;
        [SerializeField] private GameUI _gameUI;
        [SerializeField] private WorldBounds _worldBounds;
        [SerializeField] private int _maxDifficulty = 3;
        

        public override void InstallBindings()
        {
            Container.Bind<ISnake>().To<Snake>().FromInstance(_snake).AsSingle();
            Container.Bind<IGameUI>().To<GameUI>().FromInstance(_gameUI).AsSingle();
            Container.Bind<IWorldBounds>().To<WorldBounds>().FromInstance(_worldBounds).AsSingle();

            Container.Bind<IDifficulty>().To<Difficulty>().AsSingle().WithArguments(_maxDifficulty);
            Container.Bind<IScore>().To<Score>().AsSingle();
        }
    }
}