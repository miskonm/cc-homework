using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Projectiles
{
    public class BulletSpawner : MonoBehaviour
    {
        private const string DEFAULT_LAYER_MASK_NAME = "Default";
        private const string PLAYER_LAYER_MASK_NAME = "PlayerBullet";
        private const string ENEMY_LAYER_MASK_NAME = "EnemyBullet";

        [SerializeField] private Bullet _prefab;
        [SerializeField] private Transform _container;
        [SerializeField] private float _prewarmCount = 10;

        private readonly Stack<Bullet> _pool = new();

        private void Awake()
        {
            Prewarm();
        }

        public Bullet Create(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            if (_pool.TryPop(out Bullet bullet))
            {
                bullet.gameObject.SetActive(true);
            }
            else
            {
                bullet = Instantiate(_prefab, _container);
            }

            int layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer(DEFAULT_LAYER_MASK_NAME),
                TeamType.Player => LayerMask.NameToLayer(PLAYER_LAYER_MASK_NAME),
                TeamType.Enemy => LayerMask.NameToLayer(ENEMY_LAYER_MASK_NAME),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null),
            };

            Vector2 directionNormalized = direction.normalized;

            bullet.Setup(directionNormalized, speed, damage, team, layer);
            bullet.SetPosition(position);
            bullet.SetRotation(Quaternion.LookRotation(directionNormalized, Vector3.forward));

            return bullet;
        }

        public void DestroyBullet(Bullet bullet)
        {
            ReturnToPool(bullet);
        }

        private void Prewarm()
        {
            for (var i = 0; i < _prewarmCount; i++)
            {
                Bullet bullet = Instantiate(_prefab, _container);
                ReturnToPool(bullet);
            }
        }

        private void ReturnToPool(Bullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _pool.Push(bullet);
        }
    }
}