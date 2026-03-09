using Game.Ships;
using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class GameOverViewController : MonoBehaviour
    {
        [SerializeField] private GameOverView _gameOverView;
        [SerializeField] private Ship _playerShip;

        private void OnEnable()
        {
            _playerShip.OnDead += PlayerDeadCallback;
        }

        private void OnDisable()
        {
            _playerShip.OnDead -= PlayerDeadCallback;
        }

        private void PlayerDeadCallback(Ship _)
        {
            _gameOverView.Show();
        }
    }
}