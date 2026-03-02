namespace Game.Combat
{
    public interface IDamageable
    {
        TeamType Team { get; }
        void ApplyDamage(int damage);
    }
}