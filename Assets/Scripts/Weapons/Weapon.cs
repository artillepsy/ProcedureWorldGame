using Core;
using Player;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float shakeTime = 0.2f;
        
        [Header("Bullet behaviour")]
        [SerializeField] private float minDamage = 20f;
        [SerializeField] private float maxDamage = 30f;
        [SerializeField] private int penetration = 1;
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
        [Header("Sound")]
        [SerializeField] private float volume = 0.3f;
        [SerializeField] private AudioClip shootAudio;
        [SerializeField] private AudioClip reloadAudio;
        [Header("Particles")] 
        [SerializeField] private ParticleSystem firePS;

        public static readonly UnityEvent<int, int, int> OnAmmoCountChange = new UnityEvent<int, int, int>();
        public static readonly UnityEvent<int> OnShoot = new UnityEvent<int>();
        
        private float _totalReloadTIme = 0;
        private float _totalRpsTime = 0;
        
        private int _totalAmmoCount;
        private int _ammoInClip = 0;
        
        private Transform _player;
        private AudioSource _src;
        private Collider _playerCollider;
        private bool _reloading = false;
        
        public bool Shooting { get; set; }

        public string GetInfo()
        {
          return "\nDamage: " + Mathf.RoundToInt((minDamage + maxDamage)/2f) +
                 "\nRPS: " + rps +
                 "\nClip: " + clipSize;
        } 
        public void AddAmmo(float percent)
        {
            int ammoCount = (int)(maxAmmo * percent);
            if (ammoCount == 0) ammoCount = 1;
            _totalAmmoCount += ammoCount;
            if (_totalAmmoCount > maxAmmo) _totalAmmoCount = maxAmmo;
            OnAmmoCountChange?.Invoke(_totalAmmoCount, clipSize, _ammoInClip);
            if(_ammoInClip == 0) StartReload();
        }

        public void StartReload()
        {
            if (_ammoInClip == clipSize) return;
            if (_totalAmmoCount == 0) return;
            _totalReloadTIme = reloadTimeInSeconds;
        }

        private void Awake()
        {
            firePS.Stop();
            _src = GetComponent<AudioSource>();
            _totalAmmoCount = maxAmmo;
            _ammoInClip = clipSize;
            Shooting = false;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            _playerCollider = _player.GetComponent<CapsuleCollider>();
            OnAmmoCountChange?.Invoke(_totalAmmoCount, clipSize, _ammoInClip);
        }

        private void Update()
        {
            if (Reloading()) return;
            if (!Shooting) return;
            if (!CanSpawnBullet()) return;
            SpawnBullet();
        }

        protected virtual void SpawnBullet()
        {
            var inst = Instantiate(bulletPrefab, fireTransform.position, Quaternion.identity);
            var deviationAngle = Random.Range(-maxDeviationAngle, maxDeviationAngle);
            var direction = Quaternion.Euler(0, deviationAngle, 0) * _player.forward;
            var dmg = Random.Range(minDamage, maxDamage);
            inst.SetUp(direction, dmg, bulletSpeed, penetration, _playerCollider);
            _ammoInClip--;
            _totalRpsTime = 1f / rps;
            OnShoot?.Invoke(_ammoInClip);
            _src.PlayOneShot(shootAudio, volume);
            firePS.Play();
            CameraShake.Inst.Shake(shakeTime);
            if (_ammoInClip == 0) StartReload();
        }

        private bool CanSpawnBullet()
        {
            if (_ammoInClip <= 0)
            {
                StartReload();
                return false;
            }
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
            _src.PlayOneShot(reloadAudio, volume);
            OnAmmoCountChange?.Invoke(_totalAmmoCount, clipSize, _ammoInClip);
            return true;
        }
    }
}