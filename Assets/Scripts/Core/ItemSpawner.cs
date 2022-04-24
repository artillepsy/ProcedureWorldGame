using System;
using System.Collections.Generic;
using Enemy.Management;
using Player;
using UnityEngine;

namespace Core
{
    public class ItemSpawner : MonoBehaviour
    {
        [Header("Spawn settings")] 
        [SerializeField] private List<ProbabilityPrefabInfo> items;
        [SerializeField] private float itemSpawnRateInSeconds = 10;
        [SerializeField] private int maxItems = 10;
        private int _totalItems = 0;

        private void Start()
        {
            ItemPicker.OnPickupItem.AddListener(() => _totalItems--);
            ItemPicker.OnPickupItem.AddListener(() => _totalItems--);
            InvokeRepeating(nameof(SpawnItems), itemSpawnRateInSeconds, itemSpawnRateInSeconds);
        }
        
        private void SpawnItems()
        {
            if (_totalItems >= maxItems) return;
            Vector3 position;
            if (SpawnUtils.Inst.FindSpawnPoint(out position) == false) return;
            var prefab = GameObjectPool.GetInfoByProbability(items).Prefab;
            Instantiate(prefab, position, Quaternion.identity);
            _totalItems++;
        }
    }
}