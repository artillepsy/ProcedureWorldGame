using UnityEngine;

namespace Core
{
    public static class TransformUtils
    {
        public static Vector3 GetRandomPoint(Vector3 center, float halfLength)
        {
            var x = Random.Range(center.x - halfLength, center.x + halfLength);
            var z = Random.Range(center.z - halfLength, center.z + halfLength);
            return new Vector3(x, 0, z);
        }
        public static Quaternion GetRandomRotation()
        {
            return Quaternion.Euler(0, Random.Range(0, 360), 0);
        }
    }
}