using Modules.Utils;
using UnityEngine;

namespace Game.Ships
{
    public class PlayerShipPositionAdjuster : MonoBehaviour
    {
        [SerializeField] private TransformBounds _playerArea;
        [SerializeField] private Ship _ship;

        private void LateUpdate()
        {
            _ship.SetPositionInstant(_playerArea.ClampInBounds(transform.position));
        }
    }
}