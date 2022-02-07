using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using Random = UnityEngine.Random;

namespace Chunks
{
    public class ObstacleScanner : ScanNoiseUtils
    {
        [Header("Scan settings")]
        [Range(10, 100)]
        [SerializeField] private int maxOverlapAttempts = 50;
        [Header("Prefabs settings")]
        [SerializeField] private List<PlacementPrefabInfo> prefabInfoList;
        [SerializeField] private int minCount = 6;
        [SerializeField] private int maxCount = 14;
        private List<CachedInfo> _cachedObstacleInfo;
        private int _obstacleCount;
        private ChunkBounds _chunkBounds;
        private bool _afterStart = false;
        private void Awake()
        {
            base.Awake();
            _chunkBounds = new ChunkBounds(transform.position, ChunkScanner.Inst.ChunkLength);
            _obstacleCount = Random.Range(minCount, maxCount);
            _cachedObstacleInfo = new List<CachedInfo>();
        }
        private void OnEnable()
        {
            if (!_afterStart) return;
            PoolHelper.GetAllFromPool(_cachedObstacleInfo, transform);
        }
        private void OnDisable()
        {
            PoolHelper.AddAllToPool(_cachedObstacleInfo);
        }
        private void Start()
        {
            _afterStart = true;
            StartCoroutine(FindFreeSpaceCoroutine());
        }
        private IEnumerator FindFreeSpaceCoroutine()
        {
            AddPointToNode(transform.position, prefabInfoList);
            int count = 0;
            for (var i = 0; i < _obstacleCount; i++)
            {
                if (TrySpawnObstacle()) count++;
                
                yield return null;
            }
            _obstacleCount = count;
        }
        private bool TrySpawnObstacle()
        {
            var rotation = ScanHelper.GetRandomRotation();
            for (int i = 0; i < maxOverlapAttempts; i++)
            {
                var position = ScanHelper.GetRandomPoint(_chunkBounds.Left, _chunkBounds.Right, _chunkBounds.Bottom,
                    _chunkBounds.Up);
                var id = GetNearestNoisePointId(position);
                var info = ObjectPool.GetPrefabInfoById(id, prefabInfoList);

                if (info == null) continue;

                var obstacleComp = info.Prefab.GetComponent<Obstacle>();
                var halfLength = ChunkScanner.Inst.ChunkLength / 2;

                if (obstacleComp.IsOutChunk(position, transform.position, halfLength)) continue;
                if (obstacleComp.IsOverlapping(position, rotation)) continue;
                
                Spawn(info.Prefab.Id, position, rotation);
                return true;
            }
            return false;
        }
        private void AddInstancesToCache(int id, Vector3 position, Quaternion rotation, GameObject inst)
        {
            var info = new CachedInfo(id, position, rotation);
            _cachedObstacleInfo.Add(info);
            info.UpdateInstTransform(inst, transform);
        }
        public void Spawn(int id, Vector3 position, Quaternion rotation)
        {
            GameObject inst = ObjectPool.Inst.GetInstanceById(id);
            if (inst == null)
            {
                inst = Instantiate(ObjectPool.Inst.GetPrefabById(id).gameObject, position, rotation);
            }
            AddInstancesToCache(id, position, rotation, inst);
        }
        private static class ScanHelper
        {
            public static Vector3 GetRandomPoint(float left, float right, float buttom, float up)
            {
                float x = Random.Range(left, right);
                float z = Random.Range(buttom, up);
                return new Vector3(x, 0, z);
            }
            public static Quaternion GetRandomRotation()
            {
                return Quaternion.Euler(0, Random.Range(0, 360), 0);
            }
        }
        private static class PoolHelper
        {
            public static void AddAllToPool(List<CachedInfo> cachedInfo)
            {
                Dictionary<GameObject, int> objectsToPool = new Dictionary<GameObject, int>();
                foreach (CachedInfo info in cachedInfo)
                {
                    objectsToPool.Add(info.Inst, info.Id);
                }
                ObjectPool.Inst.AddInstances(objectsToPool);
            }
            public static void GetAllFromPool(List<CachedInfo> cachedInfo, Transform parent)
            {
                foreach (CachedInfo info in cachedInfo)
                {
                    GameObject inst = ObjectPool.Inst.GetInstanceById(info.Id);
                    if (inst == null)
                    {
                        inst = Instantiate(ObjectPool.Inst.GetPrefabById(info.Id).gameObject, info.Position, info.Rotation);
                    }
                    info.UpdateInstTransform(inst, parent);
                }
            }
        }
        private class ChunkBounds
        {
            private readonly float _left;
            private readonly float _right;
            private readonly float _up;
            private readonly float _bottom;
            public float Left => _left;
            public float Right => _right;
            public float Up => _up;
            public float Bottom => _bottom;
            public ChunkBounds(Vector3 center, float chunkLength)
            {
                float chunkHalfLength = chunkLength / 2;
                _left = center.x - chunkHalfLength;
                _right = center.x + chunkHalfLength;
                _up = center.z + chunkHalfLength;
                _bottom = center.z - chunkHalfLength;
            }
        }
        private class CachedInfo
        {
            private readonly int _id;
            private GameObject _inst;
            private Vector3 _position;
            private Quaternion _rotation;
            public int Id => _id;
            public GameObject Inst => _inst;
            public Vector3 Position => _position;
            public Quaternion Rotation => _rotation;
            public CachedInfo(int id, Vector3 position, Quaternion rotation)
            {
                _id = id;
                _position = position;
                _rotation = rotation;
            }
            public void UpdateInstTransform(GameObject inst, Transform parent)
            {
                _inst = inst;
                inst.transform.position = _position;
                inst.transform.rotation = _rotation;
                inst.transform.SetParent(parent);
                inst.SetActive(true);
            }
            // add object state (because same objects may be in different chunks)
        }
    }
}
