using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyHealth")]
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        public static readonly UnityEvent OnEnemyDie = new UnityEvent();
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                OnEnemyDie?.Invoke();
                Destroy(gameObject);
            }
        }
    }
}