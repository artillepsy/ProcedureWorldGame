using Experience;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class GrenadeThrower : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private float grenadeReload = 3f;
        [SerializeField] private Transform grenadePrefab;
        private int _count = 0;
        private bool _grenadePrepared = true;
        public float GrenadeReloadTime => grenadeReload;
        public int Count => _count;
        public readonly UnityEvent<int> OnGrenadeCountChange = new UnityEvent<int>();

        public void AddGrenade(int countToAdd)
        {
            _count += countToAdd;
            OnGrenadeCountChange?.Invoke(_count);
        }

        public bool SpawnGrenade()
        {
            if (!_grenadePrepared) return false;
            if (_count <= 0) return false;
            
            Instantiate(grenadePrefab, hand.position, transform.rotation);
            _grenadePrepared = false;
            Invoke(nameof(PrepareGrenade), grenadeReload);
            
            _count--;
            OnGrenadeCountChange?.Invoke(_count);
            
            return true;
        }

        private void Start()
        {
            _count = SaveSystem.Load().GrenadeCount;
            OnGrenadeCountChange?.Invoke(_count);
        }

        private void PrepareGrenade() => _grenadePrepared = true;
        
    }
}