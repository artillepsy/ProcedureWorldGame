using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [Header("Bullet behaviour")]
        [SerializeField] private float damage = 4f;
        [SerializeField] private float bulletSpeed = 15f;
        [SerializeField] private float maxDeviationAngle = 3f;
        [Header("Ammo settings")]
        [SerializeField] private float reloadTimeInSeconds = 3f;
        [SerializeField] private float rps = 30f;
        [SerializeField] private int clipSize = 20;
        [SerializeField] private int maxAmmo = 200;
        [Header("Other settings")]
        [Space]
        [SerializeField] private Transform fireTransform;
        [SerializeField] private Bullet bulletPrefab;
        
        public static readonly UnityEvent<int, int, int> OnAmmoCountChange = new UnityEvent<int, int, int>();
        public static readonly UnityEvent<int> OnShoot = new UnityEvent<int>();
        
        private float _totalReloadTIme = 0;
        private float _totalRpsTime = 0;
        
        private int _totalAmmoCount = 99999;
        private int _ammoInClip = 0;
        
        private Transform _player;
        private Coroutine _reloadCoroutine;
        private bool _reloading = false;
        public bool Shooting { get; set; }

        public void StartReload()
        {
            if (_ammoInClip == clipSize) return;
            if (_totalAmmoCount == 0) return;
            _totalReloadTIme = reloadTimeInSeconds;
        }

        private void Awake()
        {
            _ammoInClip = clipSize;
            Shooting = false;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            OnAmmoCountChange?.Invoke(_totalAmmoCount, clipSize, _ammoInClip);
        }

        private void Update()
        {
            if (Reloading()) return;
            if (!Shooting) return;
            if (!CanSpawnBullet()) return;
            SpawnBullet();
        }

        private void SpawnBullet()
        {
            var inst = Instantiate(bulletPrefab, fireTransform.position, Quaternion.identity);
            var deviationAngle = Random.Range(0, maxDeviationAngle);
            var direction = Quaternion.Euler(0, deviationAngle, 0) * _player.forward;
            inst.SetUp(direction, damage, bulletSpeed);
            _ammoInClip--;
            _totalRpsTime = 1f / rps;
            OnShoot?.Invoke(_ammoInClip);
            if (_ammoInClip == 0) StartReload();
        }

        private bool CanSpawnBullet()
        {
            if (_totalRpsTime <= 0) return true;
            _totalRpsTime -= Time.deltaTime;
            return false;
        }

        private bool Reloading()
        {
            if (_totalReloadTIme <= 0) return false;
            
            _totalReloadTIme -= Time.deltaTime;
            if (_totalReloadTIme > 0) return true;
            
            var needAmount = clipSize - _ammoInClip;

            if (_totalAmmoCount <= needAmount)
            {
                _ammoInClip += _totalAmmoCount;
                _totalAmmoCount = 0;
            }
            else
            {
                _totalAmmoCount -= needAmount;
                _ammoInClip = clipSize;
            }
            OnAmmoCountChange?.Invoke(_totalAmmoCount, clipSize, _ammoInClip);
            return true;
        }
    }
}