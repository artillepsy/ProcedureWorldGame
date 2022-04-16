using Player;
using UnityEngine;

namespace UI
{
    public class Input : MonoBehaviour
    {
        [Header("Shooting")]
        [SerializeField] private Joystick movementJoystick;
        [Range(0, 1)] 
        [SerializeField] private float fireStartInputValue = 0.8f;
        [Header("Movement")]
        [SerializeField] private Joystick shootingJoystick;
        [Range(0, 1)]
        [SerializeField] private float movementStartValue = 0.2f;
        
        private bool _inputEnapled = true;
        private PlayerMovement _playerMovement;
        private PlayerShooting _playerShooting;

        public void OnClickReload() => _playerShooting.Reload();
        public void OnClickThrowGrenade() => _playerShooting.SpawnGrenade();

        private void OnEnable()
        {
            CustomEventHandler.OnInputPermissionChanged.AddListener(status => _inputEnapled = status);
        }

        private void Start()
        {
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _playerShooting = FindObjectOfType<PlayerShooting>();
        }

        private void FixedUpdate()
        {
            if (!_inputEnapled) return;
            MovementInput();
            ShootingInput();
        }
        
        private void MovementInput()
        {
            var xAxis = movementJoystick.Horizontal;
            var zAxis = movementJoystick.Vertical;
            var direction = new Vector3(xAxis, 0, zAxis);
            if (direction.magnitude > movementStartValue)
            {
                direction.Normalize();
            }
            else
            {
                direction = Vector3.zero;
            }
            _playerMovement.UpdateDirection(direction);
        }

        private void ShootingInput()
        {
            var direction = new Vector3(shootingJoystick.Horizontal,0,shootingJoystick.Vertical);
            
            var shootingStatus = direction.magnitude > fireStartInputValue;
            _playerShooting.SetShootingStatus(shootingStatus);
            _playerShooting.UpdateDirection(direction);
        }
    }
}