using Player;
using UnityEngine;

namespace Items
{
    public class Ammo : Item
    {
        [Range(0, 1)]
        [SerializeField] private float minAmmoPercent = 0.1f;
        [Range(0, 1)]
        [SerializeField] private float maxAmmoPercent = 0.2f;
        
        
        public override void Use(Transform player)
        {
            var percent = Random.Range(minAmmoPercent, maxAmmoPercent);
            player.GetComponent<PlayerShooting>().CurrentWeapon.AddAmmo(percent);
            Destroy(gameObject);
        }
    }
}