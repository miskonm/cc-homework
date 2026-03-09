using System;
using UnityEngine;

namespace Game.Ships
{
    public abstract class Motor : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;

        private Vector2? _direction;

        public event Action<Vector3> OnMoved;

        private void FixedUpdate()
        {
            if (!_direction.HasValue)
            {
                return;
            }

            Vector2 direction = _direction.Value;
            Move(direction);
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

        private void Move(Vector2 direction)
        {
            Vector2 newPosition = _rigidbody.position + direction * (_speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
        }
    }
}