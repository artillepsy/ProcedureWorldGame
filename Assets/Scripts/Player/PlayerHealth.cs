using UnityEngine;

namespace Player
{
    [AddComponentMenu("Player/PlayerHealth")]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Debug.Log("Player is dead");
            }
        }
    }
}