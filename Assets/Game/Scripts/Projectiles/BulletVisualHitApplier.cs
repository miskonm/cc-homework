using UnityEngine;

namespace Game.Projectiles
{
    public class BulletVisualHitApplier : MonoBehaviour
    {
        [SerializeField] private BulletEffectsConfig _effectsConfig;

        public void ApplyHit(Bullet bullet)
        {
            // Explosion Vfx
            GameObject prefab = _effectsConfig.ExplosionVFX;
            Instantiate(prefab, bullet.transform.position, prefab.transform.rotation);
        }
    }
}