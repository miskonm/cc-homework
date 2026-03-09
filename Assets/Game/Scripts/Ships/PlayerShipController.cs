using UnityEngine;

namespace Game.Ships
{
    // +
    public sealed class PlayerShipController : MonoBehaviour
    {
        [SerializeField] private Ship _ship;

        private void Awake()
        {
            _ship.InitStartValues();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ship.Fire(Vector3.up);
            }

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            if (_ship.Health.Current > 0)
            {
                _ship.MoveStep(new Vector2(dx, dy));
            }
        }
    }
}