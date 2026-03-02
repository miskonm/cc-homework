using Game.Projectiles;

namespace Game.Combat
{
    public class DefaultCombatCondition : ICombatCondition
    {
        public bool CanDamage(Bullet bullet, IDamageable target)
        {
            return bullet.Team != target.Team;
        }
    }
}