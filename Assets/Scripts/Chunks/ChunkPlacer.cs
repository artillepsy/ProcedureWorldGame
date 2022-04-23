using System.Collections.Generic;
using UnityEngine;
using Core;
using Player;

namespace Chunks
{
    public class ChunkPlacer : MonoBehaviour
    {
        [Header("Global parent")]
        [SerializeField] private Transform environement;
        [Header("Prefabs settings")]
        [SerializeField] private List<ObstaclePrefabInfo> prefabInfoList;
        private Dictionary<Vector3, GameObject> _spawnedChunks;
        [Header("Scan settings")]
        [SerializeField] private float chunkLength = 40;
        [SerializeField] private float scanRateInSeconds = 1f;
        [Min(3)]
        [SerializeField] private int enableDimension = 3;
        [Min(5)]
        [SerializeField] private int disableDimension = 5;
        
        public float ChunkLength => chunkLength;
        private NoiseUtils _noiseUtils;
        private Transform _player;
        public static ChunkPlacer Inst = null;
        private int _minEnableRadius;
        private int _maxEnableRadius;
        private void Awake()
        {
            _noiseUtils = GetComponent<NoiseUtils>();
            _spawnedChunks = new Dictionary<Vector3, GameObject>();
            if (Inst == null) Inst = this;
            _minEnableRadius = (disableDimension - enableDimension) / 2;
            _maxEnableRadius = (disableDimension + enableDimension) / 2;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            InvokeRepeating(nameof(Scan), 0, scanRateInSeconds);
        }

        private void Scan()
        {
            _noiseUtils.AddPointToNode(_player.position ,prefabInfoList);
            ScanAreaInGrid(_player.position);
        }
        
        private void ScanAreaInGrid(Vector3 playerPosition)
        {
            int i, j;
            float x, z;
            var start = NoiseUtilsHelper.GetConvertedStartPoint(playerPosition, disableDimension, ChunkLength);

            for (x = start.x, i = 0; i < disableDimension; i++, x = start.x + ChunkLength * i)
            {
                for (z = start.z, j = 0; j < disableDimension; j++, z = start.z + ChunkLength * j)
                {
                    var checkPoint = new Vector3(x, 0, z);

                    if (NoiseUtilsHelper.IsInEnableArea(i, j, _minEnableRadius, _maxEnableRadius))
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
            var newId = _noiseUtils.GetNearestNoisePointId(position);
            var prefab = GameObjectPool.GetInfoById(newId, prefabInfoList).Prefab;
            var chunk = Instantiate(prefab, position, Quaternion.identity);
            chunk.transform.SetParent(environement.transform);
            _spawnedChunks.Add(position, chunk.gameObject);
        }
    }
}
