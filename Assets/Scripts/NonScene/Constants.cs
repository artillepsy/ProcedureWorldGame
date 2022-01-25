using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public static class Tags
    {
        public static string MainCamera => "MainCamera";
        public static string Player => "Player"; 
        public static string Canvas => "Canvas";
        public static string Terrain => "Terrain";
        public static string ChunkObject =>  "ChunkObject";
        public static string SceneManager => "SceneManager";
    }
    public static class PunEvent
    {
        public const byte Code = 1;

        public const int SpawnChunks = 1;
        public const int SpawnObstacles = 2;
    }

    public static class InputAsixes
    {
        public static string Horizontal => "Horizontal";
        public static string Vertical => "Vertical";
    }

    public static class LayerMasks
    {
        public static LayerMask Obstacle => LayerMask.GetMask(ChunkObstacle);
        private static string ChunkObstacle => "ChunkObstacle";
    }

    public static class Commands
    {
        public const string PlayerSpeed = "playerspeed";
    }
}
