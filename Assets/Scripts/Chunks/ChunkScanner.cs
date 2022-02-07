using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core;

namespace Chunks
{
    public class ChunkScanner : ScanNoiseUtils
    {
        [Header("Global parent")]
        [SerializeField] private Transform environement;
        [Header("Prefabs settings")]
        [SerializeField] private List<PlacementPrefabInfo> prefabInfoList;
        private Dictionary<Vector3, GameObject> _spawnedChunks;
        [Header("Scan settings")]
        [SerializeField] private float chunkLength = 100;
        [SerializeField] private float scanRateInSeconds = 0.2f;
        [Min(3)]
        [SerializeField] private int enableDimension = 3;
        [Min(5)]
        [SerializeField] private int disableDimension = 5;
        
        public float ChunkLength => chunkLength;
        public int EnableDimension => enableDimension;
        public int DisableDimension => disableDimension;
        public float ScanRateInSeconds => scanRateInSeconds;

        public static ChunkScanner Inst = null;
        private int _minEnableRadius;
        private int _maxEnableRadius;
        private void Awake()
        {
            base.Awake();
            _spawnedChunks = new Dictionary<Vector3, GameObject>();
            if (Inst == null) Inst = this;
            _minEnableRadius = (DisableDimension - EnableDimension) / 2;
            _maxEnableRadius = (DisableDimension + EnableDimension) / 2;
        }
        private void Start()
        {
            StartCoroutine(ScanTerrainCoroutine());
        }
        private IEnumerator ScanTerrainCoroutine()
        {
            while (true)
            {
                AddPointToNode(_player.position ,prefabInfoList);
                ScanAreaInGrid(_player.position);
                yield return new WaitForSeconds(ScanRateInSeconds);
            }
        }
        private void ScanAreaInGrid(Vector3 playerPosition)
        {
            int i, j;
            float x, z;
            var start = VectorHelper.GetConvertedStartPoint(playerPosition, DisableDimension, ChunkLength);

            for (x = start.x, i = 0; i < DisableDimension; i++, x = start.x + ChunkLength * i)
            {
                for (z = start.z, j = 0; j < DisableDimension; j++, z = start.z + ChunkLength * j)
                {
                    var checkPoint = new Vector3(x, 0, z);

                    if (VectorHelper.IsInEnableArea(i, j, _minEnableRadius, _maxEnableRadius))
                    {
                        if (_spawnedChunks.TryGetValue(checkPoint, out var chunk))
                        {
                            chunk.SetActive(true);
                        }
                        else
                        {
                            SpawnChunk(checkPoint); 
                        }
                    }
                    else if (_spawnedChunks.ContainsKey(checkPoint))
                    {
                        if(!_spawnedChunks.TryGetValue(checkPoint, out var chunk)) continue;
                        chunk.SetActive(false);
                    }
                }
            }
        }
        private void SpawnChunk(Vector3 position)
        {
            var newId = GetNearestNoisePointId(position);
            var prefab = ObjectPool.GetPrefabInfoById(newId, prefabInfoList).Prefab;
            var chunk = Instantiate(prefab, position, Quaternion.identity);
            chunk.transform.SetParent(environement.transform);
            _spawnedChunks.Add(position, chunk.gameObject);
        }
    }
}
