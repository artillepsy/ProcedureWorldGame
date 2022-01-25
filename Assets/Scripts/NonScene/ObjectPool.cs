using System.Collections.Generic;
using UnityEngine;

public static class ObjectPool
{
    
    //GO and id
    private static Dictionary<GameObject, int> _objects = new Dictionary<GameObject, int>();
    public static GameObject Get(int id)
    {
        if (!_objects.ContainsValue(id)) return null;

        foreach(KeyValuePair<GameObject, int> entry in _objects)
        {
            if(entry.Value == id)
            {
                _objects.Remove(entry.Key);
                return entry.Key;
            }
        }
        return null;
    }
    public static void Add(Dictionary<GameObject, int> newObjects)
    {
        foreach(KeyValuePair<GameObject, int> entry in newObjects)
        {
            if (_objects.ContainsKey(entry.Key)) Debug.LogError("Object pool contains reference to same gameObject");
            else if (entry.Key == null) Debug.LogError("Null reference to an object");

            _objects.Add(entry.Key, entry.Value);
            entry.Key.SetActive(false);
        }
    }
}
