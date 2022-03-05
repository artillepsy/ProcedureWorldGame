using UnityEngine;
using Weapons;

namespace Player
{
    [AddComponentMenu("Player/Shooting")]
    public class PlayerShooting : MonoBehaviour
    {
        [SerializeField] private Weapon weapon;
        private Vector3 _prevDirection = Vector3.forward;

        public void Reload() => weapon.StartReload();

        public void SetShootingStatus(bool status) => weapon.Shooting = status;

        public void UpdateDirection(Vector3 direction)
        {
            if (direction != Vector3.zero) _prevDirection = direction;
            else direction = _prevDirection;
            transform.LookAt(transform.position + direction, Vector3.up);
        }
    }
}
