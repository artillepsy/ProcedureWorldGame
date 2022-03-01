using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class GameObjectPool : MonoBehaviour
    {
        [SerializeField] private List<UniqueInfo> prefabs;
        private Dictionary<GameObject, int> _instances;
        public static GameObjectPool Inst = null;
        
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
        public UniqueInfo GetPrefabById(int id)
        {
            foreach (UniqueInfo prefab in prefabs)
            {
                if(prefab.Id != id) continue;
                return prefab;
            }
            return null;
        }
        public static T GetInfoById<T>(int prefabId, List<T> prefabInfoList) where T: BasePrefabInfo
        {
            foreach (T prefabInfo in prefabInfoList)
            {
                if(prefabInfo.Prefab.Id != prefabId) continue;
                return prefabInfo;
            }
            return null;
        }
        public static T GetInfoByProbability<T>(List<T> prefabInfoList) where T : ProbabilityPrefabInfo
        {
            float probabilitySum = 0;

            foreach (T info in prefabInfoList)
            {
                probabilitySum += info.Probability;
            }
            var randomPoint = Random.value * probabilitySum;

            foreach (T info in prefabInfoList)
            {
                if (randomPoint < info.Probability) return info;
                else randomPoint -= info.Probability;
            }
            return prefabInfoList[prefabInfoList.Count - 1];
        }
        public void AddAll(IEnumerable<CachedTransformInfo> cachedInfo)
        {
            foreach (var info in cachedInfo)
            {
                _instances.Add(info.Inst, info.Id);
                info.Inst.SetActive(false);
            }
        }
        public void GetAll(IEnumerable<CachedTransformInfo> cachedInfo, Transform parent)
        {
            foreach (CachedTransformInfo info in cachedInfo)
            {
                var inst = GetInstanceById(info.Id);
                if (inst == null)
                {
                    inst = Instantiate(GetPrefabById(info.Id).gameObject, info.Position, info.Rotation);
                }
                info.UpdateInstTransform(inst, parent);
            }
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