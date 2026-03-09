using Game.Common;
using Game.Projectiles;
using UnityEngine;

namespace Game.Ships
{
    public class ShipAttack : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private BulletManager _bulletManager;

        [Header("Settings")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireCooldown;
        [SerializeField] private BulletConfig _config;

        [Header("Effects")]
        [SerializeField] private AudioClip _fireSFX;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioSource _audioSource;

        private float _fireTime;

        public void Construct(BulletManager bulletManager)
        {
            _bulletManager = bulletManager;
        }

        public void Fire(Vector2 direction, TeamType teamType)
        {
            float time = Time.time;
            if (time - _fireTime < _fireCooldown)
            {
                return;
            }

            if (_fireSFX)
            {
                _audioSource.PlayOneShot(_fireSFX);
            }

            if (_fireVFX)
            {
                _fireVFX.Play();
            }

            _fireTime = time;

            CreateBullet(direction, teamType);
        }

        private void CreateBullet(Vector2 direction, TeamType teamType)
        {
            _bulletManager.Spawn(
                _firePoint.position,
                direction,
                _config,
                teamType
            );
        }
    }
}