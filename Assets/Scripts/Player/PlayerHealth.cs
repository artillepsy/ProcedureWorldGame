using System;
using UI;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [AddComponentMenu("Player/PlayerHealth")]
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float health = 100f;
        public readonly UnityEvent<float, float> OnTakeDamage = new UnityEvent<float,float>();

        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Debug.Log("player's dead"); 
            }
            OnTakeDamage?.Invoke(health, maxHealth);
        }
    }
}