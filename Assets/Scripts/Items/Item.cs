using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private float timeToDestroyInSeconds = 240f;

        private void Start()
        {
            Debug.Log("Spawned");
            Destroy(gameObject, timeToDestroyInSeconds);
        }
    }
}