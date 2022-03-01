using UnityEngine;

namespace Core
{
    public class CachedTransformInfo
    {
        public GameObject Inst { get; private set; }
        public int Id { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public CachedTransformInfo(int id, Vector3 position, Quaternion rotation)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
        }
        public void UpdateInstTransform(GameObject inst, Transform parent)
        {
            Inst = inst;
            inst.transform.position = Position;
            inst.transform.rotation = Rotation;
            inst.transform.SetParent(parent);
            inst.SetActive(true);
        }
        // add object state (because same objects may be in different chunks)
    }
}