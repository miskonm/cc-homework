using System;
using System.Collections.Generic;
using Modules;

namespace Client.Game.Coins
{
    public interface ICoinSpawner
    {
        event Action OnAllCollected;

        List<ICoin> Coins { get; }
        
        void Spawn(int count);
        void Remove(ICoin coin);
    }
}