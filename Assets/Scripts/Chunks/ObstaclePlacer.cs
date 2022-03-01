using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Chunks
{
    public class ObstaclePlacer : MonoBehaviour
    {
        [Header("Scan settings")]
        [Range(10, 100)]
        [SerializeField] private int maxOverlapAttempts = 50;
        [Header("Placement settings")]
        [SerializeField] private List<ObstaclePrefabInfo> prefabInfoList;
        [SerializeField] private int minCount = 6;
        [SerializeField] private int maxCount = 14;
        
        public readonly UnityEvent OnPlaced = new UnityEvent(); 
        private NoiseUtils _noiseUtils;
        private PlacementUtils _placementUtils;

        private int _obstacleCount;
        
        
        private void Awake()
        {
            _noiseUtils = GetComponent<NoiseUtils>();
            _placementUtils = GetComponent<PlacementUtils>();
            
            _obstacleCount = Random.Range(minCount, maxCount);
        }
        private void Start()
        {
            StartCoroutine(FindFreeSpaceCoroutine());
        }
        private IEnumerator FindFreeSpaceCoroutine()
        {
            _noiseUtils.AddPointToNode(transform.position, prefabInfoList);
            int count = 0;
            for (var i = 0; i < _obstacleCount; i++)
            {
                if (TrySpawnObstacle()) count++;
                yield return null;
            }
            _obstacleCount = count;
            OnPlaced?.Invoke();
        }
        private bool TrySpawnObstacle()
        {
            var rotation = TransformUtils.GetRandomRotation();
            for (var i = 0; i < maxOverlapAttempts; i++)
            {
                var position = TransformUtils.GetRandomPoint(transform.position, _placementUtils.HalfChunkLength);
                var id = _noiseUtils.GetNearestNoisePointId(position);
                var info = GameObjectPool.GetInfoById(id, prefabInfoList);

                if (info == null) continue;

                var obstacle = info.Prefab.GetComponent<Obstacle>();
                if (obstacle.OutOfChunk(position, transform.position, _placementUtils.HalfChunkLength)) continue;
                if (obstacle.Overlapping(position, rotation)) continue;

                _placementUtils.SpawnObstacle(id, position, rotation);
                return true;
            }
            return false;
        }
    }
}
