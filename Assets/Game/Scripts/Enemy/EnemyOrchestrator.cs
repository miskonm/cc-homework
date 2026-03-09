using System;
using Game.Ships;
using UnityEngine;

namespace Game.Enemy
{
    // +
    public sealed class EnemyOrchestrator : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private EnemySpawner _spawner;
        [SerializeField] private EnemyPositions _positions;

        private int _destroyedEnemies;

        public event Action<int> OnEnemyDead;

        public void Spawn()
        {
            Ship ship = _spawner.Spawn(_positions.GetNextSpawnPosition(), _positions.GetNextDestination());
            ship.OnDead += ShipDeadCallback;
        }

        private void ShipDeadCallback(Ship ship)
        {
            ship.OnDead -= ShipDeadCallback;

            _destroyedEnemies++;
            OnEnemyDead?.Invoke(_destroyedEnemies);
        }
    }
}