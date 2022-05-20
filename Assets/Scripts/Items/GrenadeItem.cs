using Player;
using UnityEngine;

namespace Items
{
    public class GrenadeItem : Item
    {
        [SerializeField] private int minCount = 2;
        [SerializeField] private int maxCount = 5;
        
        
        public override void Use(Transform player)
        {
            player.GetComponent<GrenadeThrower>().AddGrenade(Random.Range(minCount, maxCount+1));
            Destroy(gameObject);
        }
    }
}