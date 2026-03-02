using UnityEngine;

namespace Game.Projectiles
{
    // +
    [CreateAssetMenu(
        fileName = "BulletViewConfig",
        menuName = "Game/New BulletViewConfig"
    )]
    public sealed class BulletEffectsConfig : ScriptableObject
    {
        [field: SerializeField]
        public GameObject ExplosionVFX  { get; private set; }
    }
}