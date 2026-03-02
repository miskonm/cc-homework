using System;
using UnityEngine;

namespace Game.Ships
{
    public abstract class Motor : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private Vector2? _direction;

        public event Action<Vector3> OnMoved;

        protected float Speed => _speed;

        private void FixedUpdate()
        {
            if (!_direction.HasValue)
            {
                return;
            }

            Vector2 direction = _direction.Value;
            OnFixedUpdate(direction);
            _direction = null;

            OnMoved?.Invoke(direction);
        }

        public void SetSpeed(float speed)
        {
            _speed = speed;
        }

        public void MoveStep(Vector2 direction)
        {
            _direction = direction;
        }

        protected abstract void OnFixedUpdate(Vector2 direction);
    }
}