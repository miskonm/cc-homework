using System;
using Zenject;

namespace Client.Game.Coins
{
    public class CoinService : ICoinService, IInitializable, IDisposable
    {
        private readonly ICoinSpawner _spawner;

        public event Action OnAllCollected;

        public CoinService(
            ICoinSpawner spawner
        )
        {
            _spawner = spawner;
        }

        public void SpawnCoins(int count)
        {
            _spawner.Spawn(count);
        }

        public void Initialize()
        {
            _spawner.OnAllCollected += AllCollectedCallback;
        }

        public void Dispose()
        {
            _spawner.OnAllCollected -= AllCollectedCallback;
        }

        private void AllCollectedCallback()
        {
            OnAllCollected?.Invoke();
        }
    }
}