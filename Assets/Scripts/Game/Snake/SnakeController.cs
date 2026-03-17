using Modules;
using UnityEngine;
using Zenject;

namespace Client.Game.Snake
{
    public class SnakeController : ITickable
    {
        private const string HORIZONTAL_AXIS_NAME = "Horizontal";
        private const string VERTICAL_AXIS_NAME = "Vertical";

        private readonly ISnake _snake;

        public SnakeController(
            ISnake snake
        )
        {
            _snake = snake;
        }

        public void Tick()
        {
            SnakeDirection direction = GetDirection(GetAxis());
            if (direction != SnakeDirection.NONE)
            {
                _snake.Turn(direction);
            }
        }

        private SnakeDirection GetDirection(Vector2 axis)
        {
            switch (axis.x)
            {
                case > 0:
                    return SnakeDirection.RIGHT;
                case < 0:
                    return SnakeDirection.LEFT;
            }

            switch (axis.y)
            {
                case > 0:
                    return SnakeDirection.UP;
                case < 0:
                    return SnakeDirection.DOWN;
            }

            return SnakeDirection.NONE;
        }

        private Vector2 GetAxis()
        {
            return new Vector2(Input.GetAxisRaw(HORIZONTAL_AXIS_NAME), Input.GetAxisRaw(VERTICAL_AXIS_NAME));
        }
    }
}