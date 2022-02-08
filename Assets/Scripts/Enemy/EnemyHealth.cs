using UnityEngine;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyHealth")]
    public class EnemyHealth : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Debug.Log("Enemy is dead");
                Destroy(gameObject);
            }
        }
    }
}