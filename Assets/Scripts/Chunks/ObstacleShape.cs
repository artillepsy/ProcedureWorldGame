using UnityEditor;
using UnityEngine;

namespace Chunks
{
    public class ObstacleShape : MonoBehaviour
    {
        [SerializeField] private float sphereRadius = 1f;
        private void OnDrawGizmosSelected()
        {
            if (transform.rotation != Quaternion.identity)
            {
                Gizmos.matrix = Matrix4x4.Rotate(transform.rotation);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, transform.lossyScale);
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, sphereRadius);
        }
    }
}
