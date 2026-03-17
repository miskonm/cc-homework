using System;
using System.Collections.Generic;
using Modules;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Client.Game.Coins
{
    public class CoinSpawner : ICoinSpawner
    {
        private readonly ICoinFactory _factory;
        private readonly IWorldBounds _bounds;

        public event Action OnAllCollected;

        public List<ICoin> Coins { get; } = new();

        public CoinSpawner(
            ICoinFactory factory,
            IWorldBounds bounds)
        {
            _factory = factory;
            _bounds = bounds;
        }

        public void Spawn(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Coins.Add(_factory.Create(GetFreePosition()));
            }
        }

        public void Remove(ICoin coin)
        {
            Coins.Remove(coin);

            if (coin is MonoBehaviour mb)
            {
                Object.Destroy(mb.gameObject);
            }

            if (Coins.Count == 0)
            {
                OnAllCollected?.Invoke();
            }
        }

        private Vector2Int GetFreePosition()
        {
            return _bounds.GetRandomPosition();
        }
    }
}