using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Chunks
{
    public class Obstacle : MonoBehaviour
    {
        [SerializeField] private List<ObstacleShape> shapeList;
        public bool OutOfChunk(Vector3 position, Vector3 chunkPosition, float chunkHalfLength)
        {
            if (shapeList.Count == 0)
            {
                Debug.LogError("There are no shapes");
                return true;
            }
            foreach (ObstacleShape shape in shapeList)
            {
                var lossyScale = shape.transform.lossyScale;
                var scaleXZ = new Vector2(lossyScale.x, lossyScale.z).magnitude/2;
                //Debug.DrawLine(position + Vector3.up, position + Vector3.up + new Vector3(0, 0, scaleXZ), Color.cyan, 30);
                var length = chunkHalfLength - scaleXZ;

                if(position.x < chunkPosition.x - length || position.x > chunkPosition.x + length)
                {
                    return true;
                }
                if (position.z < chunkPosition.z - length || position.z > chunkPosition.z + length)
                {
                    return true;
                }
            }
            return false;
             
        }
        public bool Overlapping(Vector3 position, Quaternion rotation)
        {
            if (shapeList.Count == 0)
            {
                Debug.LogError("There are no shapes");
                return true;
            }
            foreach (ObstacleShape shape in shapeList)
            {
                var shapeTransform = shape.transform;
                Vector3 overlapBoxPosition = position + shapeTransform.localPosition;
                Quaternion overlapBoxRotation = rotation * shapeTransform.localRotation;
                //Debug.DrawLine(overlapBoxPosition, overlapBoxPosition + Vector3.up * 3, Color.blue, 50);
                var overlappedColliders = Physics.OverlapBox(overlapBoxPosition, shapeTransform.lossyScale, overlapBoxRotation); // anchor parent rotation add
                foreach (Collider otherCollider in overlappedColliders)
                {
                    if (otherCollider.GetComponentInParent<Obstacle>() || otherCollider.GetComponentInParent<PlayerMovement>())
                    {
                        //Debug.DrawLine(overlapBoxPosition, overlapBoxPosition + Vector3.up * 15, Color.red, 50);
                        return true;
                    }
                }
            }
            return false;
        }
        private void Awake()
        {
            foreach(ObstacleShape shape in shapeList)
            {
                if(shape == null)
                {
                    Debug.LogError("Null ref to a shape");
                }
            }
        }
    }
}

