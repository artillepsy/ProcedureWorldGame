using System.Collections.Generic;
using Chunks;
using Core;
using Player;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy.Management
{
    // add item spawn
    public class SpawnerManager : MonoBehaviour
    {
        [Header("Spawn settings")] 
        [SerializeField] private List<ProbabilityPrefabInfo> items;
        
        [SerializeField] private int maxEnemies = 10;
        [SerializeField] private int maxItems = 10;
        [SerializeField] private float spawnRateInSeconds = 6;
        [SerializeField] private float itemSpawnRateInSeconds = 10;
        [Min(0)]
        [SerializeField] private float minSpawnRadius;
        [Min(0)]
        [SerializeField] private float maxSpawnRadius;

        [SerializeField] private float sampleRadius = 10f;
        [SerializeField] private bool drawGizmos = true;
        private Rigidbody _player;
        private List<EnemyTypesContainer> _enemyTypeContainerList;
        private int _totalEnemies = 0;
        private int _totalItems = 0;
        public static SpawnerManager Inst = null;
        public void AddContainerToList(EnemyTypesContainer enemyTypesContainer)
        {
            _enemyTypeContainerList.Add(enemyTypesContainer);
        }
        public void RemoveContainerFromList(EnemyTypesContainer enemyTypesContainer)
        {
            _enemyTypeContainerList.Remove(enemyTypesContainer);
        }
        private void Awake()
        {
            Inst = this;
            _enemyTypeContainerList = new List<EnemyTypesContainer>();
        }
        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().GetComponent<Rigidbody>();
            EnemyHealth.OnEnemyDie.AddListener(()=> _totalEnemies--);
            EnemyBehaviour.OnEnemyTooFar.AddListener(()=> _totalEnemies--);
            InvokeRepeating(nameof(SpawnEnemies), spawnRateInSeconds, spawnRateInSeconds);
            InvokeRepeating(nameof(SpawnItems), itemSpawnRateInSeconds, itemSpawnRateInSeconds);
        }

        private void SpawnEnemies()
        {
            if (_totalEnemies >= maxEnemies) return;
            if (!TryGetPrefabToSpawn(out var position, out var enemy)) return;
            Instantiate(enemy, position, Quaternion.identity);
            _totalEnemies++;
        }

        private void SpawnItems()
        {
            if (_totalItems >= maxItems) return;
            Vector3 position;
            if (FindSpawnPoint(out position) == false) return;
            var prefab = GameObjectPool.GetInfoByProbability(items).Prefab;
            Instantiate(prefab, position, Quaternion.identity);
            _totalItems++;
        }

        private bool TryGetPrefabToSpawn(out Vector3 position, out GameObject prefab)
        {
            prefab = null;
            if (FindSpawnPoint(out position) == false) return false;
            var chunkPosition = ConvertPointToChunkPosition(position);
            var uniqueId = GetPrefabFromEnemyContainer(chunkPosition);
           // Debug.DrawLine(chunkPosition, chunkPosition + Vector3.up*5, Color.red, 30);
            if (uniqueId == null) return false;
            prefab = uniqueId.gameObject;
            return true;
        }
        private Vector3 ConvertPointToChunkPosition(Vector3 pos)
        {
            var chunkLength = ChunkPlacer.Inst.ChunkLength;
            var centerX = Mathf.RoundToInt(pos.x / chunkLength) * chunkLength;
            var centerZ = Mathf.RoundToInt(pos.z / chunkLength) * chunkLength;
            return new Vector3(centerX, 0, centerZ);
        }
        private UniqueInfo GetPrefabFromEnemyContainer(Vector3 chunkPosition)
        {
            foreach (var container in _enemyTypeContainerList)
            {
                if(container.transform.position != chunkPosition) continue;
                return container.GetPrefabByProbability();
            }
            return null;
        }
        private bool FindSpawnPoint(out Vector3 position)
        {
            Vector3 samplePos;
            position = Vector3.zero;
            var randomRadius = Random.Range(minSpawnRadius, maxSpawnRadius);
            var randomAngle = Random.Range(0, 360);
            if (_player.velocity != Vector3.zero)
            {
                samplePos = _player.velocity.normalized * randomRadius;
                samplePos =_player.position + Quaternion.Euler(0, Random.Range(-10f, 10f), 0) * samplePos;
            }
            else
            {
                samplePos = Vector3.forward * randomRadius;
                samplePos = _player.position +  Quaternion.Euler(0, randomAngle, 0) * samplePos;
            }
            var result = NavMesh.SamplePosition(samplePos, out var hit, sampleRadius, 1);
            if (!result) return false;
            position = hit.position;
           // Debug.DrawLine(position, position + Vector3.up*5, Color.yellow, 30);
            return true;
        }
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos) return;
            var position = transform.position;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(position, new Vector3(minSpawnRadius*2, 0, minSpawnRadius*2));
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(position, new Vector3(maxSpawnRadius*2, 0, maxSpawnRadius*2));
        }
    }
}
