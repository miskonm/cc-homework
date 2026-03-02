using System;
using Game.Ships;
using Modules.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    // +
    public sealed class EnemyOrchestrator : MonoBehaviour
    {
        [Header("Spawn")]
        [SerializeField] private float _minSpawnCooldown = 2;
        [SerializeField] private float _maxSpawnCooldown = 3;
        [SerializeField] private EnemySpawner _spawner;

        [Header("Target")]
        [SerializeField] private ShipController _player;

        [Header("Points")]
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;

        private float _spawnCooldown;
        private float _spawnTime;
        private int _spawnIndex;
        private int _attackIndex;
        private int _destroyedEnemies;

        public event Action<int> OnEnemyDead;

        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }

        private void Start()
        {
            ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - _spawnTime < _spawnCooldown || _player.CurrentHealth <= 0)
            {
                return;
            }

            Spawn();
        }

        private void Spawn()
        {
            ShipController ship = _spawner.Spawn(NextSpawnPosition(), NextDestination(), _player);
            ship.OnDead += ShipDeadCallback;

            ResetSpawnCooldown();
        }

        private void ShipDeadCallback(ShipController ship)
        {
            ship.OnDead -= ShipDeadCallback;

            _destroyedEnemies++;
            OnEnemyDead?.Invoke(_destroyedEnemies);
        }

        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }

        private Vector3 NextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        private Vector3 NextDestination()
        {
            if (_attackIndex >= _attackPositions.Length)
            {
                _attackPositions.Shuffle();
                _attackIndex = 0;
            }

            return _attackPositions[_attackIndex++].position;
        }
    }
}