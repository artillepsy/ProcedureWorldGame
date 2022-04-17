using Player;
using UnityEngine;

namespace Items
{
    public class Ammo : Item
    {
        [SerializeField] private int minAmmo = 10;
        [SerializeField] private int maxAmmo = 20;
        
        public override void Use(Transform player)
        {
            var count = Random.Range(minAmmo, maxAmmo + 1);
            player.GetComponent<PlayerShooting>().CurrentWeapon.AddAmmo(count);
        }
    }
}