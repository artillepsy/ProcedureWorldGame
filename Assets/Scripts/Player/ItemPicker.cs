using Items;
using UnityEngine;

namespace Player
{
    public class ItemPicker : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger) return;
            if (!other.GetComponentInParent<Item>()) return;
            
            Debug.Log("Picked");
            Destroy(other.gameObject);
        }
    }
}