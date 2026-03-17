using System;
using Client.Game;
using Modules;
using Zenject;

namespace Client.UI
{
    public class GameUIPresenter : IInitializable, IDisposable
    {
        private readonly IGameUI _gameUI;
        private readonly IScore _score;
        private readonly IDifficulty _difficulty;
        private readonly IGameService _gameService;

        public GameUIPresenter(
            IGameUI gameUI,
            IScore score,
            IDifficulty difficulty,
            IGameService gameService
        )
        {
            _gameUI = gameUI;
            _score = score;
            _difficulty = difficulty;
            _gameService = gameService;
        }

        public void Initialize()
        {
            _score.OnStateChanged += ScoreStateChangedCallback;
            _difficulty.OnStateChanged += DifficultyStateChangedCallback;
            _gameService.OnGameFinished += GameFinishedCallback;

            DifficultyStateChangedCallback();
            ScoreStateChangedCallback(_score.Current);
        }

        public void Dispose()
        {
            _score.OnStateChanged -= ScoreStateChangedCallback;
            _difficulty.OnStateChanged -= DifficultyStateChangedCallback;
            _gameService.OnGameFinished -= GameFinishedCallback;
        }

        private void GameFinishedCallback(bool isWin)
        {
            _gameUI.GameOver(isWin);
        }

        private void DifficultyStateChangedCallback()
        {
            _gameUI.SetDifficulty(_difficulty.Current, _difficulty.Max);
        }

        private void ScoreStateChangedCallback(int value)
        {
            _gameUI.SetScore(value.ToString());
        }
    }
}