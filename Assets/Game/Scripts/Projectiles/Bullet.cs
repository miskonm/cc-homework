using System;
using UnityEngine;

namespace Game.Projectiles
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;

        public event Action<Bullet, Collider2D> OnTriggerEntered;

        public TeamType Team { get; private set; }
        public Vector2 Direction { get; private set; }
        public Vector3 Position => transform.position;
        public int Damage { get; private set; }
        public float Speed { get; private set; }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEntered?.Invoke(this, other);
        }

        public void Setup(Vector2 direction, float speed, int damage, TeamType team, int layer)
        {
            Direction = direction;
            Speed = speed;
            Damage = damage;
            Team = team;
            gameObject.layer = layer;
            ActivateTeamVfx(team);
        }

        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        private void ActivateTeamVfx(TeamType team)
        {
            if (team == TeamType.Player)
            {
                _blueVFX.SetActive(true);
                _redVFX.SetActive(false);
            }
            else
            {
                _blueVFX.SetActive(false);
                _redVFX.SetActive(true);
            }
        }
    }
}