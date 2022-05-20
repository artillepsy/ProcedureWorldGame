using Experience;
using TimeManagement;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class TimeFreezer : MonoBehaviour
    {
        [SerializeField] private float freezeTime = 5f;
        [SerializeField] private float reloadTime = 10f;

        private bool _ready = true;
        private int _count = 0;
        

        public int Count => _count;
        public float ReloadTime => reloadTime;
        public readonly UnityEvent<int> OnFreezersCountChange = new UnityEvent<int>();

        public bool FreezeTime()
        {
            if (!_ready) return false;
            if (_count <= 0) return false;
            _ready = false;
            _count--;
            OnFreezersCountChange?.Invoke(_count);
            Invoke(nameof(Reload), reloadTime);
            TimeManager.Inst.FreezeTime(freezeTime);
            return true;
        }
        
        public void AddTimeFreezer(int countToAdd)
        {
            _count += countToAdd;
            OnFreezersCountChange?.Invoke(_count);
        }
        
        private void Start()
        {
            
            _count = SaveSystem.Load().FreezersCount;
            OnFreezersCountChange?.Invoke(_count);
        }
        
        private void Reload() => _ready = true;
    }
}