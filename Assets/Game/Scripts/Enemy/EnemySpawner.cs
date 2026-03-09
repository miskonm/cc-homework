using System.Collections;
using System.Collections.Generic;
using Game.Common;
using Game.Projectiles;
using Game.Ships;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Ship _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private BulletManager _bulletManager;
        [SerializeField] private VfxSpawner _vfxSpawner;

        [Header("Target")]
        [SerializeField] private Ship _player;

        private readonly Queue<Ship> _pool = new();

        public Ship Spawn(Vector3 spawnPosition, Vector3 destinationPosition)
        {
            if (_pool.TryDequeue(out Ship ship))
            {
                ship.gameObject.SetActive(true);
            }
            else
            {
                ship = Instantiate(_prefab, _container);
                ship.GetComponent<ShipAttack>().Construct(_bulletManager);
                ship.GetComponent<ShipView>().Construct(_vfxSpawner);
            }

            ship.InitStartValues();
            ship.SetPositionInstant(spawnPosition);

            var enemy = ship.GetComponent<Ships.Enemy>();
            enemy.SetDestinationPosition(destinationPosition);
            enemy.SetTarget(_player);

            ship.OnDead += EnemyDeadCallback;

            return ship;
        }

        private void EnemyDeadCallback(Ship ship)
        {
            ship.OnDead -= EnemyDeadCallback;
            Despawn(ship);
        }

        private void Despawn(Ship ship)
        {
            StartCoroutine(DespawnInNextFrame(ship));
        }

        private IEnumerator DespawnInNextFrame(Ship ship)
        {
            yield return null;
            ship.gameObject.SetActive(false);
            _pool.Enqueue(ship);
        }
    }
}