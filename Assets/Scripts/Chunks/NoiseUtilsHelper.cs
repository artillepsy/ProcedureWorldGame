using UnityEngine;

namespace Chunks
{
    public static class NoiseUtilsHelper
    {
        public static bool IsInEnableArea(int i, int j, int minOutRadius, int maxOutRadius)
        {
            return (i >= minOutRadius && i < maxOutRadius && j >= minOutRadius && j < maxOutRadius);
        }

        public static Vector3 GetConvertedStartPoint(Vector3 pos, int outDimension, float nodeSize)
        {
            var offsetSteps = (outDimension - 1) / 2;
            var centerX = Mathf.RoundToInt((pos.x - offsetSteps * nodeSize) / nodeSize) * nodeSize;
            var centerZ = Mathf.RoundToInt((pos.z - offsetSteps * nodeSize) / nodeSize) * nodeSize;
            return new Vector3(centerX, 0, centerZ);
        }

        public static Vector3 GetRandomPointInBounds(Vector3 point, float halfSize)
        {
            var left = point.x - halfSize;
            var right = point.x + halfSize;
            var up = point.z + halfSize;
            var bottom = point.z - halfSize;
            
            var x = Random.Range(left, right);
            var z = Random.Range(bottom, up);
            return new Vector3(x, 0, z);
        }
    }
}