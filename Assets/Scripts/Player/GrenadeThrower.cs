using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class GrenadeThrower : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private float grenadeReload = 3f;
        [SerializeField] private Transform grenadePrefab;
        [SerializeField] private int count = 0;
        private bool _grenadePrepared = true;
        public float GrenadeReloadTime => grenadeReload;
        public readonly UnityEvent<int> OnGrenadeCountChange = new UnityEvent<int>();

        public void AddGrenade(int countToAdd)
        {
            count += countToAdd;
            OnGrenadeCountChange?.Invoke(count);
        }

        public bool SpawnGrenade()
        {
            if (!_grenadePrepared) return false;
            if (count <= 0) return false;
            
            Instantiate(grenadePrefab, hand.position, transform.rotation);
            _grenadePrepared = false;
            Invoke(nameof(PrepareGrenade), grenadeReload);
            
            count--;
            OnGrenadeCountChange?.Invoke(count);
            
            return true;
        }

        private void Start()
        {
            OnGrenadeCountChange?.Invoke(count);
        }

        private void PrepareGrenade() => _grenadePrepared = true;
        
    }
}