using System.Collections.Generic;
using System.Collections;
using Chunks;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class ItemPlacer : MonoBehaviour
    {
        [Header("Scan settings")]
        [Range(10, 100)]
        [SerializeField] private int maxOverlapAttempts = 50;
        [Space]
        [SerializeField] private int minCount;
        [SerializeField] private int maxCount;
        [SerializeField] private List<ItemPrefabInfo> prefabInfoList;
        private int _itemCount; 
        private PlacementUtils _placementUtils;
        private void Awake()
        {
            _placementUtils = GetComponent<PlacementUtils>();
            _itemCount = Random.Range(minCount, maxCount+1);
            GetComponent<ObstaclePlacer>().OnPlaced.AddListener(()=> StartCoroutine(FindFreeSpaceCoroutine()));
        }

        private IEnumerator FindFreeSpaceCoroutine()
        {
            int count = 0;
            for (var i = 0; i < _itemCount; i++)
            {
                if (TrySpawnItem()) count++;
                yield return null;
            }
            _itemCount = count;
        }
        private bool TrySpawnItem()
        {
            for (var i = 0; i < maxOverlapAttempts; i++)
            {
                var position = TransformUtils.GetRandomPoint(transform.position, _placementUtils.HalfChunkLength);
                var prefab = GameObjectPool.GetInfoByProbability(prefabInfoList).Prefab;

                var obstacle = prefab.GetComponent<Obstacle>();
                if (obstacle.Overlapping(position, Quaternion.identity)) continue;
                Instantiate(prefab, position, Quaternion.identity);
                return true;
            }
            return false;
        }
    }
}