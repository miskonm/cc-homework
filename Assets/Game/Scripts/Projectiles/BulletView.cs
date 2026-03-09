using Game.Common;
using UnityEngine;

namespace Game.Projectiles
{
    public sealed class BulletView : MonoBehaviour
    {
        [SerializeField] private Bullet _bullet;

        [SerializeField] private GameObject _blueVFX;
        [SerializeField] private GameObject _redVFX;
        [SerializeField] private BulletEffectsConfig _effectsConfig;

        private void OnEnable()
        {
            _bullet.OnHit += BulletHitCallback;
            _bullet.OnTeamChanged += BulletTeamChangedCallback;
        }

        private void OnDisable()
        {
            _bullet.OnHit -= BulletHitCallback;
            _bullet.OnTeamChanged -= BulletTeamChangedCallback;
        }

        private void BulletTeamChangedCallback(Bullet bullet, TeamType team)
        {
            ActivateTeamVfx(team);
        }

        private void BulletHitCallback(Bullet bullet)
        {
            ApplyHit(bullet);
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

        private void ApplyHit(Bullet bullet)
        {
            // Explosion Vfx
            GameObject prefab = _effectsConfig.ExplosionVFX;
            Instantiate(prefab, bullet.transform.position, prefab.transform.rotation);
        }
    }
}