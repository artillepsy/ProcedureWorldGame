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
            EnemySpawnerManager.Inst.AddContainerToList(this);
        }
        private void OnDisable()
        {
            EnemySpawnerManager.Inst.RemoveContainerFromList(this);
        }
    }
}