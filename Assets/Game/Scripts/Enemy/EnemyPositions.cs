using Modules.Utils;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyPositions : MonoBehaviour
    {
        [Header("Points")]
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private Transform[] _attackPositions;

        private int _spawnIndex;
        private int _attackIndex;

        private void Awake()
        {
            _spawnPositions.Shuffle();
            _attackPositions.Shuffle();
        }

        public Vector3 GetNextSpawnPosition()
        {
            if (_spawnIndex >= _spawnPositions.Length)
            {
                _spawnPositions.Shuffle();
                _spawnIndex = 0;
            }

            return _spawnPositions[_spawnIndex++].position;
        }

        public Vector3 GetNextDestination()
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