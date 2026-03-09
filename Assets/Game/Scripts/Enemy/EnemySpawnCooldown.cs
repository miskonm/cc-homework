using Game.Ships;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawnCooldown : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private Ship _player;
        [SerializeField] private EnemyOrchestrator _orchestrator;

        [SerializeField] private float _minSpawnCooldown = 2;
        [SerializeField] private float _maxSpawnCooldown = 3;

        private float _spawnCooldown;
        private float _spawnTime;

        private void Start()
        {
            ResetSpawnCooldown();
        }

        private void FixedUpdate()
        {
            float time = Time.fixedTime;
            if (time - _spawnTime < _spawnCooldown || _player.Health.Current <= 0)
            {
                return;
            }

            _orchestrator.Spawn();
            ResetSpawnCooldown();
        }

        private void ResetSpawnCooldown()
        {
            _spawnCooldown = Random.Range(_minSpawnCooldown, _maxSpawnCooldown);
            _spawnTime = Time.fixedTime;
        }
    }
}