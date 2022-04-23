using Player;
using UnityEngine;

namespace Items
{
    public class Heal : Item
    {
        [SerializeField] private float healAmount;
        
        public override void Use(Transform player)
        {
            player.GetComponent<PlayerHealth>().ChangeHealth(healAmount, false);
            Destroy(gameObject);
        }
    }
}