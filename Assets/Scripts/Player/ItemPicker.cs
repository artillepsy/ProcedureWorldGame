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
            var comp = other.GetComponentInParent<Item>();
            if (!comp) return;
            
            comp.Use(transform);
            _src.PlayOneShot(pickAudio, volume);
        }
    }
}