using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy.Management
{
    public class EnemyTypesContainer : MonoBehaviour
    {
        [SerializeField] private List<EnemyPrefabInfo> prefabInfoList;

        public UniqueInfo GetPrefabByProbability()
        {
            return GameObjectPool.GetInfoByProbability(prefabInfoList).Prefab;
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