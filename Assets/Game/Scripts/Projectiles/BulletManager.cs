using System;
using System.Collections.Generic;
using Game.Common;
using Modules.Utils;
using UnityEngine;

namespace Game.Projectiles
{
    // +
    public sealed class BulletManager : MonoBehaviour
    {
        private const string DEFAULT_LAYER_MASK_NAME = "Default";
        private const string PLAYER_LAYER_MASK_NAME = "PlayerBullet";
        private const string ENEMY_LAYER_MASK_NAME = "EnemyBullet";

        [SerializeField] private BulletSpawner _spawner;
        [SerializeField] private TransformBounds _levelBounds;

        private readonly List<Bullet> _bullets = new();

        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];

                if (!_levelBounds.InBounds(bullet.Position))
                {
                    DestroyBullet(bullet);
                }
            }
        }

        public void Spawn(Vector2 position, Vector2 direction, BulletConfig config, TeamType team)
        {
            Bullet bullet = _spawner.Create();

            int layer = team switch
            {
                TeamType.None => LayerMask.NameToLayer(DEFAULT_LAYER_MASK_NAME),
                TeamType.Player => LayerMask.NameToLayer(PLAYER_LAYER_MASK_NAME),
                TeamType.Enemy => LayerMask.NameToLayer(ENEMY_LAYER_MASK_NAME),
                _ => throw new ArgumentOutOfRangeException(nameof(team), team, null),
            };

            Vector2 directionNormalized = direction.normalized;

            bullet.Setup(directionNormalized, config, team, layer);
            bullet.SetPosition(position);
            bullet.SetRotation(Quaternion.LookRotation(directionNormalized, Vector3.forward));

            bullet.OnHit += HitCallback;

            _bullets.Add(bullet);
        }

        private void DestroyBullet(Bullet bullet)
        {
            bullet.OnHit -= HitCallback;

            _bullets.Remove(bullet);
            _spawner.DestroyBullet(bullet);
        }

        private void HitCallback(Bullet bullet)
        {
            DestroyBullet(bullet);
        }
    }
}