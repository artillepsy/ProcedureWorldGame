using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private List<UniqueId> prefabs;
        private Dictionary<GameObject, int> _instances;
        public static GameObjectPool Inst = null;
        public void AddInstances(Dictionary<GameObject, int> activeInstances)
        {
            foreach (var (key, value) in activeInstances)
            {
                _instances.Add(key, value);
                key.SetActive(false);
            }
        }
        public GameObject GetInstanceById(int id)
        {
            if (!_instances.ContainsValue(id)) return null;
            foreach (var (key, value) in _instances)
            {
                if (value != id) continue;
                _instances.Remove(key);
                return key;
            }
            return null;
        }
        public UniqueId GetPrefabById(int id)
        {
            foreach (UniqueId prefab in prefabs)
            {
                if(prefab.Id != id) continue;
                return prefab;
            }
            return null;
        }
        public static T GetPrefabInfoById<T>(int prefabId, List<T> prefabInfoList) where T: BasePrefabInfo
        {
            foreach (T prefabInfo in prefabInfoList)
            {
                if(prefabInfo.Prefab.Id != prefabId) continue;
                return prefabInfo;
            }
            return null;
        }
        public static int GetIdByProbability<T>(List<T> prefabInfoList) where T: ProbabilityPrefabInfo
        {
            float probabilitySum = 0;

            foreach (T info in prefabInfoList)
            {
                probabilitySum += info.Probability;
            }
            var randomPoint = Random.value * probabilitySum;

            foreach (T info in prefabInfoList)
            {
                if (randomPoint < info.Probability) return info.Prefab.Id;
                else randomPoint -= info.Probability;
            }
            return prefabInfoList[prefabInfoList.Count - 1].Prefab.Id;
        }
        public static UniqueId GetPrefabByProbability<T>(List<T> prefabInfoList) where T : ProbabilityPrefabInfo
        {
            float probabilitySum = 0;

            foreach (T info in prefabInfoList)
            {
                probabilitySum += info.Probability;
            }
            var randomPoint = Random.value * probabilitySum;

            foreach (T info in prefabInfoList)
            {
                if (randomPoint < info.Probability) return info.Prefab;
                else randomPoint -= info.Probability;
            }
            return prefabInfoList[prefabInfoList.Count - 1].Prefab;
        }
        
        private void Awake()
        {
            if (Inst == null) Inst = this;
            _instances = new Dictionary<GameObject, int>();
            for (var i = 0; i < prefabs.Count; i++)
            {
                prefabs[i].Id = i;
            }
        }
    }
}