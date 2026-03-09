using System;
using Game.Combat;
using Game.Common;
using UnityEngine;

namespace Game.Ships
{
    // +
    public class Ship : MonoBehaviour, IDamageable
    {
        [SerializeField] private ShipControllerSO _config;
        [SerializeField] private ShipAttack _shipAttack;
        [SerializeField] private ShipHealth _health;
        [SerializeField] private Motor _motor;
        [SerializeField] private TeamType _teamType;

        public event Action<Ship> OnDead;
        public event Action OnHit;

        public Vector3 MoveDirection { get; private set; }
        public TeamType Team => _teamType;
        public ShipHealth Health => _health;

        public void InitStartValues()
        {
            _health.Init(_config.Health);
            _motor.SetSpeed(_config.MoveSpeed);
        }

        public void ApplyDamage(int damage)
        {
            if (!_health.ApplyDamage(damage))
            {
                return;
            }

            if (_health.Current > 0)
            {
                OnHit?.Invoke();
            }
            else
            {
                NotifyAboutDead();
            }
        }

        public void SetPositionInstant(Vector3 position)
        {
            transform.position = position;
        }

        public void MoveStep(Vector2 direction)
        {
            _motor.MoveStep(direction);
            MoveDirection = direction;
        }

        public void Fire(Vector2 direction)
        {
            _shipAttack.Fire(direction, Team);
        }

        private void NotifyAboutDead()
        {
            OnDead?.Invoke(this);
        }
    }
}