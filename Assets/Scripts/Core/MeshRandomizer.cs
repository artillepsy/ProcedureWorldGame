using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class MeshRandomizer : MonoBehaviour
    {
        [SerializeField] private List<GameObject> objects;

        private void Awake()
        {
            objects.ForEach(obj => obj.SetActive(false));
            objects[Random.Range(0, objects.Count)].SetActive(true);
        }
    }
}