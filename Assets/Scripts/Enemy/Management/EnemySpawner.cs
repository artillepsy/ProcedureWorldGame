using UnityEngine;
using UnityEngine.Events;

namespace Enemy.Management
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Count")]
        [SerializeField] private int minStartCount = 3;
        [SerializeField] private int maxStartCount = 5;
        [SerializeField] private int incrementPerWave = 5;
        [Header("Time")]
        [SerializeField] private float timeOfWave = 35f;
        
        [SerializeField] private float spawnRate = 1f;
        private int _needToSpawn = 0;
        private int _waveNumber = 0;
        public static readonly UnityEvent<int> OnStartWave = new UnityEvent<int>(); 
        
        private void Start()
        {
            InvokeRepeating(nameof(SpawnEnemy), spawnRate, spawnRate);
            Invoke(nameof(StartWave), 5f);
        }

        private void StartWave()
        {
            var min = minStartCount + _waveNumber * incrementPerWave;
            var max = maxStartCount + _waveNumber * incrementPerWave + 1;
            _needToSpawn += Random.Range(min, max);
            _waveNumber++;
            OnStartWave?.Invoke(_waveNumber);
        }


        private void SpawnEnemy()
        {
            if (_needToSpawn == 0) return;
            if (!SpawnUtils.Inst.TryGetPrefabToSpawn(out var position, out var enemy)) return;
            Instantiate(enemy, position, Quaternion.identity);
            _needToSpawn--;
            //  CancelInvoke(nameof(StartWave));
            if (_needToSpawn > 0) return;
            Invoke(nameof(StartWave), timeOfWave + _waveNumber * incrementPerWave);
        }
    }
}