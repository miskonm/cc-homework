using UnityEngine;

namespace Game.Ships
{
    public class ShipAttackWithCooldown : ShipAttack
    {
        [Header("Settings")]
        [SerializeField] private Transform _firePoint;
        [SerializeField] private float _fireCooldown;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private int _bulletDamage;

        [Header("Effects")]
        [SerializeField] private AudioClip _fireSFX;
        [SerializeField] private ParticleSystem _fireVFX;
        [SerializeField] private AudioSource _audioSource;

        private float _fireTime;

        public override void Fire(Vector2 direction, TeamType teamType)
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
            BulletManager.Spawn(
                _firePoint.position,
                direction,
                _bulletSpeed,
                _bulletDamage,
                teamType
            );
        }
    }
}