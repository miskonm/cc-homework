using UnityEngine;

namespace Game.Ships
{
    // +
    public sealed class Enemy : ShipController
    {
        [Header("Enemy")]
        [SerializeField] private float _stoppingDistance = 0.25f;

        [SerializeField] private ShipController _target;
        [SerializeField] private Vector2 _destination;

        private float _fireTime;

        public override TeamType Team => TeamType.Enemy;

        private void FixedUpdate()
        {
            if (CurrentHealth <= 0 || _target == null || _target.CurrentHealth <= 0)
            {
                return;
            }

            Vector2 distance = _destination - (Vector2)transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            if (isNotReached)
            {
                MoveStep(distance.normalized);
            }
            else
            {
                Fire(_target.transform.position - transform.position);
            }
        }

        public void SetDestinationPosition(Vector3 destinationPosition)
        {
            _destination = destinationPosition;
        }

        public void SetTarget(ShipController target)
        {
            _target = target;
        }
    }
}