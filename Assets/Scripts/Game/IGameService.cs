using System;

namespace Client.Game
{
    public interface IGameService
    {
        event Action<bool> OnGameFinished;
    }
}