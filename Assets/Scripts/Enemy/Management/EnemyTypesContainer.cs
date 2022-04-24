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
            SpawnUtils.Inst.AddContainerToList(this);
        }
        private void OnDisable()
        {
            SpawnUtils.Inst.RemoveContainerFromList(this);
        }
    }
}