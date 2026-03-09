using Game.Ships;
using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class PlayerHealthViewController : MonoBehaviour
    {
        [SerializeField] private HealthView _healthView;
        [SerializeField] private Ship _playerShip;

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
            _healthView.SetHealth(health, _playerShip.Health.Max);
        }
    }
}