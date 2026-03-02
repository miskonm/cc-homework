using System.Collections;
using System.Collections.Generic;
using Game.Projectiles;
using Game.Ships;
using UnityEngine;

namespace Game
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Enemy _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private BulletManager _bulletManager;
        [SerializeField] private VfxSpawner _vfxSpawner;

        private readonly Queue<Enemy> _pool = new();

        public Enemy Spawn(Vector3 spawnPosition, Vector3 destinationPosition, ShipController target)
        {
            if (_pool.TryDequeue(out Enemy enemy))
            {
                enemy.gameObject.SetActive(true);
            }
            else
            {
                enemy = Instantiate(_prefab, _container);
                enemy.GetComponent<ShipAttack>().Construct(_bulletManager);
                enemy.Construct(_vfxSpawner);
            }

            enemy.InitStartValues();
            enemy.SetPositionInstant(spawnPosition);
            enemy.SetDestinationPosition(destinationPosition);
            enemy.SetTarget(target);

            enemy.OnDead += EnemyDeadCallback;

            return enemy;
        }

        private void EnemyDeadCallback(ShipController ship)
        {
            ship.OnDead -= EnemyDeadCallback;
            Despawn(ship);
        }

        private void Despawn(ShipController ship)
        {
            StartCoroutine(DespawnInNextFrame(ship));
        }

        private IEnumerator DespawnInNextFrame(ShipController ship)
        {
            yield return null;
            ship.gameObject.SetActive(false);
            _pool.Enqueue(ship as Enemy);
        }
    }
}