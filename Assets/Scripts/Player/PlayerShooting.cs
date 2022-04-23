using UnityEngine;
using Weapons;

namespace Player
{
    [AddComponentMenu("Player/Shooting")]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Weapon weaponPrefab;
        [SerializeField] private float grenadeReload = 4f;
        [SerializeField] private Transform grenadePrefab;
        private Vector3 _prevDirection = Vector3.forward;
        private bool _grenadePrepared = true;
        private Weapon _weapon;
        public Weapon CurrentWeapon=> _weapon;
        public float GrenadeReloadTime => grenadeReload;

        public void ChangeWeapon(Weapon newPrefab)
        {
            _weapon.transform.SetParent(null);
            Destroy(_weapon.gameObject);
            _weapon = newPrefab;
            _weapon = Instantiate(_weapon, hand.position, hand.localRotation, hand);
        }

        public bool SpawnGrenade()
        {
            if (!_grenadePrepared) return false;
            Instantiate(grenadePrefab, hand.position, transform.rotation);
            _grenadePrepared = false;
            Invoke(nameof(PrepareGrenade), grenadeReload);
            return true;
        }

        public void Reload() => _weapon.StartReload();

        public void SetShootingStatus(bool status) => _weapon.Shooting = status;

        public void UpdateDirection(Vector3 direction)
        {
            if (direction != Vector3.zero) _prevDirection = direction;
            else direction = _prevDirection;
            transform.LookAt(transform.position + direction, Vector3.up);
        }

        private void Start() => _weapon = Instantiate(weaponPrefab, hand.position, hand.localRotation, hand);

        private void PrepareGrenade() => _grenadePrepared = true;
    }
}
