using UnityEngine;

[System.Serializable]
public class PrefabInfo
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private int _id;
    [Space]
    [Range(0, 1)]
    [SerializeField] private float _probability;
    [Range(0, 1)]
    [SerializeField] private float _minPerlinValue;
    [Range(0, 1)]
    [SerializeField] private float _maxPerlinValue;

    public GameObject Prefab => _prefab;
    public int Id => _id;
    public float Probability => _probability;
    public float MinPerlinValue => _minPerlinValue;
    public float MaxPerlinValue => _maxPerlinValue;
}

