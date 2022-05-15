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
        public readonly UnityEvent<float, float> OnHealthChanged = new UnityEvent<float,float>();
        public static readonly UnityEvent OnDamaged = new UnityEvent();
        public static readonly UnityEvent OnDied = new UnityEvent();

        public void ChangeHealth(float amount, bool isDamage = true)
        {
            health += isDamage ? -amount : amount;
            if (isDamage)
            {
                _src.PlayOneShot(damagedAudios[Random.Range(0, damagedAudios.Count)], volume);
                OnDamaged?.Invoke();
                damagedPS.Play();    
            }
            if (health <= 0)
            {
                health = 0;
                OnDied?.Invoke();
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
            OnHealthChanged?.Invoke(health, maxHealth);
        }

        private void Awake() => _src = GetComponent<AudioSource>();
    }
}