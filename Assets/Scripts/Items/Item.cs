using TMPro;
using UnityEngine;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private float timeToDestroyInSeconds = 50;
        [SerializeField] protected TextMeshPro label;
        [SerializeField] protected string itemName;

        public abstract void Use(Transform player);
        
        protected void Start()
        {
            label.text = itemName;
            Destroy(gameObject, timeToDestroyInSeconds);
        }
    }
}