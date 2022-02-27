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
            public static Vector3 GetRandomPointInBounds(float left, float right, float buttom, float up)
            {
                var x = Random.Range(left, right);
                var z = Random.Range(buttom, up);
                return new Vector3(x, 0, z);
            }
    }
}