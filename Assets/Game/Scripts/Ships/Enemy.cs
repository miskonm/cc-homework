using UnityEngine;

namespace Game.Ships
{
    // +
    public sealed class Enemy : MonoBehaviour
    {
        [Header("Enemy")]
        [SerializeField] private Ship _ownShip;
        [SerializeField] private float _stoppingDistance = 0.25f;
        [SerializeField] private Ship _target;
        [SerializeField] private Vector2 _destination;

        private float _fireTime;

        private void FixedUpdate()
        {
            if (_ownShip.Health.Current <= 0 || _target == null || _target.Health.Current <= 0)
            {
                return;
            }

            Vector2 distance = _destination - (Vector2)transform.position;
            bool isNotReached = distance.sqrMagnitude > _stoppingDistance * _stoppingDistance;

            if (isNotReached)
            {
                _ownShip.MoveStep(distance.normalized);
            }
            else
            {
                _ownShip.Fire(_target.transform.position - transform.position);
            }
        }

        public void SetDestinationPosition(Vector3 destinationPosition)
        {
            _destination = destinationPosition;
        }

        public void SetTarget(Ship target)
        {
            _target = target;
        }
    }
}