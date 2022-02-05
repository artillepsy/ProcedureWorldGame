using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy.Entity
{
    public class EnemySpawnerManager : MonoBehaviour
    {
        [SerializeField] private int maxEnemiesForPlayer = 10;
        [SerializeField] private float spawnRateInSeconds = 6;
        
        private int _maxEnemies;
        private int _totalEnemies;

        private void Awake()
        {
            _totalEnemies = 0;
        }

        private void OnEnable()
        {
            PlayerObserver.OnPlayersFound.AddListener(OnPlayersFound);
        }
        private void OnDisable()
        {
            PlayerObserver.OnPlayersFound.RemoveListener(OnPlayersFound);
        }
        private void OnPlayersFound(List<Transform> players)
        {
            _maxEnemies = maxEnemiesForPlayer * players.Count;
            StartCoroutine(SpawnCoroutine());
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                if(_totalEnemies >= _maxEnemies) continue;
                var uniqueId = EnemySpawnDataManager.Inst.GetPreparedToSpawnUniqueId(out var point);
                if(uniqueId == null) continue;
                SpawnEnemy(uniqueId, point);
                yield return new WaitForSeconds(spawnRateInSeconds);
            }
        }
        private void SpawnEnemy(UniqueId uniqueId, Vector3 point)
        {
            var instance = ObjectPool.Inst.GetInstanceById(uniqueId.Id);
            if (instance == null)
            {
                 instance = Instantiate(uniqueId, point, Quaternion.identity).gameObject;
            }
        }
    }
}