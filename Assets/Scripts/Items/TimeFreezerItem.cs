using Player;
using UnityEngine;

namespace Items
{
    public class TimeFreezerItem : Item
    {
        [SerializeField] private int minCount = 1;
        [SerializeField] private int maxCount = 1;
        
        public override void Use(Transform player)
        {
            player.GetComponent<TimeFreezer>().AddTimeFreezer(Random.Range(minCount, maxCount+1));
            Destroy(gameObject);
        }
    }
}