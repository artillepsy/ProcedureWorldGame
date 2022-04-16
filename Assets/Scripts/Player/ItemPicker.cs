using Items;
using UnityEngine;

namespace Player
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private AudioClip pickAudio;
        [SerializeField] private float volume = 1f;
        private AudioSource _src;

        private void Awake() => _src = GetComponent<AudioSource>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger) return;
            if (!other.GetComponentInParent<Item>()) return;
            
            _src.PlayOneShot(pickAudio, volume);
            Debug.Log("Picked");
            Destroy(other.gameObject);
        }
    }
}