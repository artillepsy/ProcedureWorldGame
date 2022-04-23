using System;
using UnityEngine;

namespace Enemy.Management
{
    public class EnemyWaves : MonoBehaviour
    {
        [Header("Count")]
        [SerializeField] private int minStartCount = 3;
        [SerializeField] private int maxStartCount = 5;
        [SerializeField] private int incCountPerWave = 5;
        [Header("Time")]
        [SerializeField] private float timeOfWave = 35f;
        [SerializeField] private float spawnRate = 1f;
        [SerializeField] private float timeBetweenWave = 10f;
        private int _totalCount;
        private int _totalKilled;
        private bool _spawnEnabled = false;
        
        private void Start()
        {
            
        }

        private void Update()
        {
            if (!_spawnEnabled) return;
            if (_totalKilled < _totalCount) return;
            
            
        }

        private void SpawnEnemy()
        {
            
        }
        
        
    }
}