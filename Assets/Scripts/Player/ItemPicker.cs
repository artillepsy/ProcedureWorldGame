using Items;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class ItemPicker : MonoBehaviour
    {
        [SerializeField] private AudioClip pickAudio;
        [SerializeField] private float volume = 1f;
        private AudioSource _src;

        public static readonly UnityEvent OnPickupItem = new UnityEvent();
        private void Awake() => _src = GetComponent<AudioSource>();
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger) return;
            var comp = other.GetComponentInParent<Item>();
            if (!comp) return;
            
            comp.Use(transform);
            OnPickupItem?.Invoke();
            _src.PlayOneShot(pickAudio, volume);
        }
    }
}