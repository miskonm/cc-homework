using UnityEngine;

namespace Game.Ships
{
    public sealed class RigidbodyMotor : Motor
    {
        [SerializeField] private Rigidbody2D _rigidbody;

        protected override void OnFixedUpdate(Vector2 direction)
        {
            Vector2 newPosition = _rigidbody.position + direction * (Speed * Time.fixedDeltaTime);
            _rigidbody.MovePosition(newPosition);
        }
    }
}