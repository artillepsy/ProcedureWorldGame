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
            var count = 0;
            for (var i = 0; i < _itemCount; i++)
            {
                TrySpawnItem();
                count++;
                yield return null;
            }
            _itemCount = count;
        }

        private void TrySpawnItem()
        {
            var position = TransformUtils.GetRandomPoint(transform.position, _placementUtils.HalfChunkLength);
            var prefab = GameObjectPool.GetInfoByProbability(prefabInfoList).Prefab;
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}