using System.Collections.Generic;
using UnityEngine;

public class ChunkObstacle : MonoBehaviour
{
    [SerializeField] private List<ObstacleShape> _obstacleShape;
    public bool Overlapping(Vector3 position, Quaternion rotation)
    {
        if (_obstacleShape.Count == 0)
        {
            Debug.LogError("There are no shapes");
            return true;
        }
        foreach(ObstacleShape shape in _obstacleShape)
        {
            Vector3 overlapPosition = position + shape.transform.position;
            Quaternion overlapRotation = rotation * shape.transform.rotation;
            Debug.DrawLine(overlapPosition, overlapPosition + new Vector3(shape.transform.lossyScale.x, 0, 0), Color.red, 30);
            Collider[] other = Physics.OverlapBox(overlapPosition, shape.transform.lossyScale, overlapRotation); // anchor parent rotation add
            foreach(Collider collider in other)
            {
                if(collider.GetComponentInParent<ChunkObstacle>() || collider.GetComponentInParent<PlayerMovement>())
                {
                    return true;
                }
            }
        }
        return false;
    }
}

