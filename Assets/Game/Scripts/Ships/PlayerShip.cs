using System;
using Modules.Utils;
using UnityEngine;

namespace Game.Ships
{
    // +
    public sealed class PlayerShip : ShipController
    {
        [SerializeField] private TransformBounds _playerArea;

        public override TeamType Team => TeamType.Player;

        private void Awake()
        {
            InitStartValues();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire(Vector3.up);
            }

            float dx = Input.GetAxisRaw("Horizontal");
            float dy = Input.GetAxisRaw("Vertical");

            if (CurrentHealth > 0)
            {
                MoveStep(new Vector2(dx, dy));
            }
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            SetPositionInstant(_playerArea.ClampInBounds(transform.position));
        }
    }
}