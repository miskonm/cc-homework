using Game.Projectiles;
using UnityEngine;

namespace Game.Ships
{
    public abstract class ShipAttack : MonoBehaviour
    {
        [Header("Services")]
        [SerializeField] private BulletManager _bulletManager;

        protected BulletManager BulletManager => _bulletManager;

        public void Construct(BulletManager bulletManager)
        {
            _bulletManager = bulletManager;
        }

        public abstract void Fire(Vector2 direction, TeamType teamType);
    }
}