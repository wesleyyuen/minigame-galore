using UnityEngine;

namespace TurnBasedRPG
{
    public class PlayerMovedSignal
    {
        public Vector2 PlayerPosition { get; }
        public PlayerMovedSignal(Vector2 playerPosition)
        {
            PlayerPosition = playerPosition;
        }
    }
}