using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private float timeToDestroyInSeconds = 240f;

        public abstract void Use(Transform player);
        
        private void Start()
        {
            Destroy(gameObject, timeToDestroyInSeconds);
        }
    }
}