using System;
using Client.Game.Coins;
using Client.Game.Lose;
using Modules;
using Zenject;

namespace Client.Game
{
    public class GameService : IGameService, IInitializable, IDisposable
    {
        private readonly IDifficulty _difficulty;
        private readonly ICoinService _coinService;
        private readonly IGameLoseService _loseService;
        private readonly ISnake _snake;

        public event Action<bool> OnGameFinished;

        public GameService(
            IDifficulty difficulty,
            ICoinService coinService,
            IGameLoseService loseService,
            ISnake snake)
        {
            _difficulty = difficulty;
            _coinService = coinService;
            _loseService = loseService;
            _snake = snake;
        }

        public void Initialize()
        {
            _coinService.OnAllCollected += AllCoinsCollectedCallback;
            _loseService.OnLose += LoseCallback;

            StartGame();
        }

        public void Dispose()
        {
            _coinService.OnAllCollected -= AllCoinsCollectedCallback;
            _loseService.OnLose -= LoseCallback;
        }

        private void StartGame()
        {
            _snake.SetActive(true);
            AllCoinsCollectedCallback();
        }

        private void NextLevel()
        {
            if (!_difficulty.Next(out int level))
            {
                SetGameOver(true);
                return;
            }

            _snake.SetSpeed(level);
            _coinService.SpawnCoins(level);
        }

        private void AllCoinsCollectedCallback()
        {
            NextLevel();
        }

        private void SetGameOver(bool isWin)
        {
            _snake.SetActive(false);
            OnGameFinished?.Invoke(isWin);
        }

        private void LoseCallback()
        {
            SetGameOver(false);
        }
    }
}