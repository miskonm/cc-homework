using Modules.UI;
using UnityEngine;

namespace Game.UI
{
    public class ScoreViewController : MonoBehaviour
    {
        [SerializeField] private EnemyOrchestrator _enemyOrchestrator;
        [SerializeField] private ScoreView _scoreView;

        private int _points;

        private void Awake()
        {
            UpdateView();
        }

        private void OnEnable()
        {
            _enemyOrchestrator.OnEnemyDead += EnemyDeadCallback;
        }

        private void OnDisable()
        {
            _enemyOrchestrator.OnEnemyDead -= EnemyDeadCallback;
        }

        private void EnemyDeadCallback(int count)
        {
            _points = count;
            UpdateView();
        }

        private void UpdateView()
        {
            _scoreView.SetValue(_points);
        }
    }
}