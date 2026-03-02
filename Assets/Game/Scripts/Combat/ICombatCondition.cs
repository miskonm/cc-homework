using Game.Projectiles;

namespace Game.Combat
{
    public interface ICombatCondition
    {
        bool CanDamage(Bullet bullet, IDamageable target);
    }
}