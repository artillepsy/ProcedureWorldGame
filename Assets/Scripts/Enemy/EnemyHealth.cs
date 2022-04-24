using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyHealth")]
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private DamageText dmgText;
        [Space]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private AudioClip deadAudio;
        [SerializeField] private AudioClip damagedAudio;
        [SerializeField] private float volume = 1f;
        [Space] 
        [SerializeField] private ParticleSystem damagedPS;
        [SerializeField] private ParticleSystem deadPSPrefab;
        [SerializeField] private Image healthImg;
        private float _health;
        private AudioSource _src;
        
        public static readonly UnityEvent OnEnemyDie = new UnityEvent();
        public readonly UnityEvent OnDie = new UnityEvent();
        public readonly UnityEvent OnTakeDmg = new UnityEvent();
        public void TakeDamage(float damage, Vector3 direction)
        {
            if (_health <= 0) return;
            _health -= damage;
            healthImg.fillAmount = _health / maxHealth;
            _src.PlayOneShot(damagedAudio, volume);
            damagedPS.Play();
            OnTakeDmg?.Invoke();
            
            var inst = Instantiate(dmgText, transform.position, quaternion.identity);
            inst.SetValues(damage, direction);
            
            if (_health <= 0)
            {
                _health = 0;
                OnEnemyDie?.Invoke();
                OnDie?.Invoke();
                Instantiate(deadPSPrefab, damagedPS.transform.position, Quaternion.identity);
                _src.PlayOneShot(deadAudio, volume);
                //Destroy(gameObject);
            }
        }

        private void Awake()
        {
            _health = maxHealth;
            damagedPS.Stop();
            _src = GetComponent<AudioSource>();
        }
    }
}