using System;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyHealth")]
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        [SerializeField] private AudioClip deadAudio;
        [SerializeField] private AudioClip damagedAudio;
        [SerializeField] private float volume = 1f;
        [Space] 
        [SerializeField] private ParticleSystem damagedPS;
        [SerializeField] private ParticleSystem deadPSPrefab;
        
        private AudioSource _src;
        
        public static readonly UnityEvent OnEnemyDie = new UnityEvent();
        public readonly UnityEvent OnDie = new UnityEvent();
        public void TakeDamage(float damage)
        {
            if (health <= 0) return;
            health -= damage;
            _src.PlayOneShot(damagedAudio, volume);
            damagedPS.Play();
            if (health <= 0)
            {
                health = 0;
                OnEnemyDie?.Invoke();
                OnDie?.Invoke();
                Instantiate(deadPSPrefab, damagedPS.transform.position, Quaternion.identity);
                _src.PlayOneShot(deadAudio, volume);
                //Destroy(gameObject);
            }
        }

        private void Awake()
        {
            damagedPS.Stop();
            _src = GetComponent<AudioSource>();
        }
    }
}