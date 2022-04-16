using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [AddComponentMenu("Player/PlayerHealth")]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float health = 100f;
        [SerializeField] private List<AudioClip> damagedAudios;
        [SerializeField] private float volume = 1f;
        [SerializeField] private ParticleSystem damagedPS;
        private AudioSource _src;
        public readonly UnityEvent<float, float> OnTakeDamage = new UnityEvent<float,float>();

        public void TakeDamage(float damage)
        {
            health -= damage;
            _src.PlayOneShot(damagedAudios[Random.Range(0, damagedAudios.Count)], volume);
            damagedPS.Play();
            if (health <= 0)
            {
                health = 0;
                Debug.Log("player's dead"); 
            }
            OnTakeDamage?.Invoke(health, maxHealth);
        }

        private void Awake() => _src = GetComponent<AudioSource>();
    }
}