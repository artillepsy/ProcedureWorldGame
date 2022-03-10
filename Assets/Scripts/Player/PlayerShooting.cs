using UnityEngine;
using Weapons;

namespace Player
{
    [AddComponentMenu("Player/Shooting")]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        [SerializeField] private Weapon weaponPrefab;
        private Vector3 _prevDirection = Vector3.forward;
        private Weapon _weapon;
        
        public void Reload() => _weapon.StartReload();

        public void SetShootingStatus(bool status) => _weapon.Shooting = status;

        public void UpdateDirection(Vector3 direction)
        {
            if (direction != Vector3.zero) _prevDirection = direction;
            else direction = _prevDirection;
            transform.LookAt(transform.position + direction, Vector3.up);
        }

        private void Start()
        {
             _weapon = Instantiate(weaponPrefab, hand.position, hand.localRotation, hand);
             
        }
    }
}
