using UnityEngine;

public static class Constants
{
    public static class Tags
    {
        public const string MainCamera = "MainCamera";
        public const string Player = "Player"; 
        public const string Canvas = "Canvas";
        public const string Terrain = "Terrain";
        public const string ChunkObject =  "ChunkObject";
        public const string SceneManager = "SceneManager";
    }
    public static class PunEvent
    {
        public const byte Code = 1;

        public const int PlaceChunks = 1;
        public const int SpawnObstacles = 2;
    }

    public static class InputAsixes
    {
        public const string Horizontal = "Horizontal";
        public const string Vertical = "Vertical";
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
        public const string ColliderEnableMode = "cem";
    }
    public static class ChunkPlacer
    {
       public const int TRUE_CODE = -1;
       public const int FALSE_CODE = -2;
    }
}
