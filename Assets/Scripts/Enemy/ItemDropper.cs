using System;
using System.Collections.Generic;
using Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class ItemDropper : MonoBehaviour
    {
        [SerializeField] private List<ItemPrefabInfo> prefabInfoList;
        [Range(0, 1)]
        [SerializeField] private float dropChance = 0.3f;
        [SerializeField] private int exp = 10;

        private void Awake()
        {
            GetComponent<EnemyHealth>().OnDie.AddListener(Drop);
        }

        private void Drop()
        {
            if (Random.value > dropChance) return;
            var prefab = GameObjectPool.GetInfoByProbability(prefabInfoList).Prefab;
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}