using Player;
using UnityEngine;
using Weapons;

namespace Items
{
    public class WeaponItem : Item
    {
        [SerializeField] private Weapon prefab;
        public override void Use(Transform player)
        {
            player.GetComponent<PlayerShooting>().ChangeWeapon(prefab);
            Destroy(gameObject);
        }
        protected void Start()
        {
            base.Start();
            label.text += prefab.GetInfo();
        }
    }
}