using System;

namespace Client.Game.Lose
{
    public interface IGameLoseService
    {
        event Action OnLose;
    }
}