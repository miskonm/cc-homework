using System;
using Game.Combat;
using Game.Common;
using UnityEngine;

namespace Game.Projectiles
{
    public sealed class Bullet : MonoBehaviour
    {
        private BulletConfig _config;
        private TeamType _team;
        private Vector2 _direction;

        public event Action<Bullet> OnHit;
        public event Action<Bullet, TeamType> OnTeamChanged;

        public Vector3 Position => transform.position;

        private void FixedUpdate()
        {
            Vector3 moveStep = _direction * (_config.Speed * Time.fixedDeltaTime);
            SetPosition(Position + moveStep);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable) || _team == damageable.Team)
            {
                return;
            }

            if (_config.Damage > 0)
            {
                damageable.ApplyDamage(_config.Damage);
            }

            OnHit?.Invoke(this);
        }

        public void Setup(Vector2 direction, BulletConfig config, TeamType team, int layer)
        {
            _direction = direction;
            _config = config;
            _team = team;
            gameObject.layer = layer;
            OnTeamChanged?.Invoke(this, _team);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}