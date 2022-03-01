using UnityEngine;

namespace Core
{
    [System.Serializable]
    public class BasePrefabInfo
    {
        [SerializeField] protected UniqueInfo prefab;
        public UniqueInfo Prefab => prefab;
    }
    [System.Serializable]
    public class ProbabilityPrefabInfo : BasePrefabInfo
    {
        [Range(0, 1)]
        [SerializeField] private float probability;
        public float Probability => probability;
    }
    [System.Serializable]
    public class ObstaclePrefabInfo : ProbabilityPrefabInfo
    {
    }
    
    [System.Serializable]
    public class EnemyPrefabInfo : ProbabilityPrefabInfo
    {
    } 
    
    [System.Serializable]
    public class ItemPrefabInfo : ProbabilityPrefabInfo
    {
    }
}

