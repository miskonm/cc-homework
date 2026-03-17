using System;
using Modules;
using UnityEngine;
using Zenject;

namespace Client.Game.Coins
{
    public class CoinCollector : IInitializable, IDisposable
    {
        private readonly ISnake _snake;
        private readonly ICoinSpawner _spawner;
        private readonly IScore _score;

        public CoinCollector(
            ISnake snake,
            ICoinSpawner spawner,
            IScore score
        )
        {
            _snake = snake;
            _spawner = spawner;
            _score = score;
        }

        public void Initialize()
        {
            _snake.OnMoved += OnMoved;
        }

        public void Dispose()
        {
            _snake.OnMoved -= OnMoved;
        }

        private void OnMoved(Vector2Int position)
        {
            for (int i = _spawner.Coins.Count - 1; i >= 0; i--)
            {
                ICoin coin = _spawner.Coins[i];

                if (coin.Position != position)
                {
                    continue;
                }

                _score.Add(coin.Score);
                _snake.Expand(coin.Bones);

                _spawner.Remove(coin);
            }
        }
    }
}