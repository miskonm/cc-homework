using Game.Ships;
using Modules.Utils;
using UnityEngine;

namespace Game.UI
{
    public class PlayerCameraShaker : MonoBehaviour
    {
        [SerializeField] private Ship _playerShip;
        [SerializeField] private CameraShaker _cameraShaker;

        private void OnEnable()
        {
            _playerShip.Health.OnChanged += PlayerHealthChangedCallback;
        }

        private void OnDisable()
        {
            _playerShip.Health.OnChanged -= PlayerHealthChangedCallback;
        }

        private void PlayerHealthChangedCallback(int health)
        {
            _cameraShaker.Shake();
        }
    }
}