using UnityEngine;

namespace Core
{
    public static class Constants
    {
        public static class Scenes
        {
            public const string Game = "Game";
            public const string MainMenu = "MainMenu";
        }
        public static class InputAsixes
        {
            public const string Horizontal = "Horizontal";
            public const string Vertical = "Vertical";
        }

        public static class Tags
        {
            public const string Player = "Player";
        }

        public static class LayerMasks
        {
            public static LayerMask Obstacle => LayerMask.GetMask(ChunkObstacle);
            private const string ChunkObstacle = "ChunkObstacle";
        }

        public static class Commands
        {
            public const string Password = "123";
            public const string PlayerSpeed = "ps";
            public const string ColliderEnableMode = "pc";
        }
        public static class Icons
        {
            public enum Type { Player, Enemy }
        }
    }
}
