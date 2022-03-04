using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Chunks
{
    public class PlacementUtils : MonoBehaviour
    {
        private List<CachedTransformInfo> _cachedTransformInfo;
        private bool _afterStart = false;

        public float HalfChunkLength { get; private set; }

        private float _leftEdge;
        private float _rightEdge;
        private float _upEdge;
        private float _bottomEdge;

        private void Awake()
        {
            HalfChunkLength = ChunkPlacer.Inst.ChunkLength / 2;
            _cachedTransformInfo = new List<CachedTransformInfo>();
        }
        
        public void SpawnObstacle(int id, Vector3 position, Quaternion rotation)
        {
            var inst = GameObjectPool.Inst.GetInstanceById(id);
            if (inst == null)
            {
                inst = Instantiate(GameObjectPool.Inst.GetPrefabById(id).gameObject, position, rotation);
            }
            
            var info = new CachedTransformInfo(id, position, rotation);
            _cachedTransformInfo.Add(info);
            info.UpdateInstTransform(inst, transform);
        }

        private void OnEnable()
        {
            if (!_afterStart) return;
            GameObjectPool.Inst.GetAll(_cachedTransformInfo, transform);
        }
        private void OnDisable()
        {
            GameObjectPool.Inst.AddAll(_cachedTransformInfo);
        }

        private void Start()
        {
            _cachedTransformInfo = new List<CachedTransformInfo>();
            _afterStart = true;
        }
    }
}