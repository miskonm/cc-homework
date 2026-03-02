using Game.Projectiles;
using UnityEngine;

namespace Game.Combat
{
    public class CombatResolver : MonoBehaviour
    {
        [SerializeField] private BulletVisualHitApplier _visualHitApplier;

        private ICombatCondition _condition;

        public void SetCondition(ICombatCondition condition)
        {
            _condition = condition;
        }

        public bool Resolve(Bullet bullet, Collider2D other)
        {
            if (!other.TryGetComponent(out IDamageable damageable) || !_condition.CanDamage(bullet, damageable))
            {
                return false;
            }

            if (bullet.Damage > 0)
            {
                damageable.ApplyDamage(bullet.Damage);
            }

            _visualHitApplier.ApplyHit(bullet);

            return true;
        }
    }
}