using UnityEngine;

namespace Game.Ships
{
    // +
    [CreateAssetMenu(menuName = "Game/ShipControllerInfo", order = 0)]
    public sealed class ShipControllerSO : ScriptableObject
    {
        [Header("Core")]
        [field: SerializeField]
        public int Health { get; private set; } = 5;

        [field: SerializeField]
        public float MoveSpeed { get; private set; } = 5;
    }
}