using UnityEngine;

namespace Game.Projectiles
{
    [CreateAssetMenu(fileName = nameof(BulletConfig), menuName = "Game/Bullet Config")]
    public class BulletConfig : ScriptableObject
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _speed;

        public int Damage => _damage;
        public float Speed => _speed;
    }
}