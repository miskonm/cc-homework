using Game.Ships;
using Modules.UI;
using Modules.Utils;
using UnityEngine;

namespace Game.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private HealthView _healthView;
        [SerializeField] private CameraShaker _cameraShaker;
        [SerializeField] private PlayerShip _playerShip;

        private void OnEnable()
        {
            _playerShip.OnHealthChanged += PlayerHealthChangedCallback;
            _playerShip.OnDead += PlayerDeadCallback;
        }

        private void OnDisable()
        {
            _playerShip.OnHealthChanged -= PlayerHealthChangedCallback;
            _playerShip.OnDead -= PlayerDeadCallback;
        }

        private void PlayerDeadCallback(ShipController _)
        {
            _gameOverView.Show();
        }

        private void PlayerHealthChangedCallback(int health)
        {
            _healthView.SetHealth(health, _playerShip.MaxHealth);
            _cameraShaker.Shake();
        }
    }
}