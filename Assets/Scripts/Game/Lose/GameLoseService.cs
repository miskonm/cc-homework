using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Client.Game.Lose
{
    public class GameLoseService : IGameLoseService, IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly IWorldBounds _bounds;

        public event Action OnLose;

        public GameLoseService(ISnake snake, IWorldBounds bounds)
        {
            _snake = snake;
            _bounds = bounds;
        }

        public void Initialize()
        {
            _snake.OnMoved += CheckBounds;
            _snake.OnSelfCollided += HandleDeath;
        }

        public void Dispose()
        {
            _snake.OnMoved -= CheckBounds;
            _snake.OnSelfCollided -= HandleDeath;
        }

        private void CheckBounds(Vector2Int pos)
        {
            if (!_bounds.IsInBounds(pos))
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            _snake.SetActive(false);
            OnLose?.Invoke();
        }
    }
}