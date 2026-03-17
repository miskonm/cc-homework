using UnityEngine;

namespace Modules
{
    //Don't modify
    public interface IWorldBounds
    {
        bool IsInBounds(Vector2Int position);
        Vector2Int GetRandomPosition();
    }
}