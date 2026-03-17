using Modules;
using UnityEngine;
using Zenject;

namespace Client.Game.Coins
{
    public class CoinFactory : ICoinFactory
    {
        private const string PREFAB_PATH = "Prefabs/Coin/Coin";

        private readonly IInstantiator _instantiator;

        private Coin _prefabInternal;

        private Coin Prefab
        {
            get
            {
                if (_prefabInternal == null)
                {
                    _prefabInternal = Resources.Load<Coin>(PREFAB_PATH);
                }

                return _prefabInternal;
            }
        }

        public CoinFactory(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }

        public ICoin Create(Vector2Int position)
        {
            var coin = _instantiator.InstantiatePrefabForComponent<Coin>(Prefab);
            coin.Position = position;
            coin.Generate();
            return coin;
        }
    }
}