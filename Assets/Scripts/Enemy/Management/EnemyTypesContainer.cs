using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy.Management
{
    public class EnemyTypesContainer : MonoBehaviour
    {
        [SerializeField] private List<EnemyPrefabInfo> prefabInfoList;

        public UniqueId GetPrefabByProbability()
        {
            return GameObjectPool.GetPrefabByProbability(prefabInfoList);
        }
        private void OnEnable()
        {
            SpawnerManager.Inst.AddContainerToList(this);
        }
        private void OnDisable()
        {
            SpawnerManager.Inst.RemoveContainerFromList(this);
        }
    }
}