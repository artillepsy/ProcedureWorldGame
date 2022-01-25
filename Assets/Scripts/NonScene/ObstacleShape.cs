using UnityEditor;
using UnityEngine;

public class ObstacleShape : MonoBehaviour
{
    [SerializeField] private float _sphereRadius = 1f;
    private void OnDrawGizmosSelected()
    {
        if (transform.rotation != Quaternion.identity)
        {
            Gizmos.matrix = Matrix4x4.Rotate(transform.rotation);
        }
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, _sphereRadius);
    }
}
