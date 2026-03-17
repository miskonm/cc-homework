using System;

namespace Client.Game.Coins
{
    public interface ICoinService
    {
        event Action OnAllCollected;

        void SpawnCoins(int count);
    }
}