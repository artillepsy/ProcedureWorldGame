using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Items
{
    public abstract class Item : MonoBehaviour
    {
        [SerializeField] private float timeToDestroyInSeconds = 50;
        [SerializeField] protected TextMeshProUGUI label;
        [SerializeField] protected string itemName;
        
        public static readonly UnityEvent OnItemDestroy = new UnityEvent();

        public abstract void Use(Transform player);
        
        protected void Start()
        {
            label.text = itemName;
            Invoke(nameof(DestroySelf), timeToDestroyInSeconds);
        }

        private void DestroySelf()
        {
            OnItemDestroy?.Invoke();
            Destroy(gameObject);
        }
    }
}