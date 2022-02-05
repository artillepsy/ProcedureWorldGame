using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy
{
    public class ChunkEnemyTypeContainer : MonoBehaviour
    {
        [SerializeField] private List<EnemyPrefabInfo> prefabInfoList;

        public UniqueId GetPrefabByProbability()
        {
            return ObjectPool.GetPrefabByProbability(prefabInfoList);
        }

        private void OnEnable()
        {
            EnemySpawnDataManager.Inst.AddContainerToList(this);
        }
        private void OnDisable()
        {
            EnemySpawnDataManager.Inst.RemoveContainerFromList(this);
        }
    }
}