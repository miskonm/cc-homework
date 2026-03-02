using System.Collections.Generic;
using Game.Combat;
using Modules.Utils;
using UnityEngine;

namespace Game.Projectiles
{
    // +
    public sealed class BulletManager : MonoBehaviour
    {
        [SerializeField] private BulletSpawner _spawner;
        [SerializeField] private TransformBounds _levelBounds;
        [SerializeField] private CombatResolver _combatResolver;

        private readonly List<Bullet> _bullets = new();

        private void Start()
        {
            _combatResolver.SetCondition(new DefaultCombatCondition());
        }

        private void FixedUpdate()
        {
            for (int i = _bullets.Count - 1; i >= 0; i--)
            {
                Bullet bullet = _bullets[i];
                Vector3 moveStep = bullet.Direction * (bullet.Speed * Time.fixedDeltaTime);
                bullet.SetPosition(bullet.Position + moveStep);

                if (!_levelBounds.InBounds(bullet.transform.position))
                {
                    DestroyBullet(bullet);
                }
            }
        }

        public void Spawn(Vector2 position, Vector2 direction, float speed, int damage, TeamType team)
        {
            Bullet bullet = _spawner.Create(position, direction, speed, damage, team);
            bullet.OnTriggerEntered += TriggerEnteredCallback;
            _bullets.Add(bullet);
        }

        private void DestroyBullet(Bullet bullet)
        {
            _bullets.Remove(bullet);

            bullet.OnTriggerEntered -= TriggerEnteredCallback;

            _spawner.DestroyBullet(bullet);
        }

        private void TriggerEnteredCallback(Bullet bullet, Collider2D other)
        {
            if (_combatResolver.Resolve(bullet, other))
            {
                DestroyBullet(bullet);
            }
        }
    }
}