using Modules;
using UnityEngine;

namespace Client.Game.Coins
{
    public interface ICoinFactory
    {
        ICoin Create(Vector2Int position);
    }
}