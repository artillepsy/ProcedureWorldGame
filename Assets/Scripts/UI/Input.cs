using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private Image grenadeBtnImg;
        [SerializeField] private Image timeFreezerBtnImg;
        [SerializeField] private float emptyImageAlpha = 0.2f;
        private bool _inputEnapled = true;
        private PlayerMovement _playerMovement;
        private PlayerShooting _playerShooting;
        private TimeFreezer _timeFreezer;
        private GrenadeThrower _grenadeThrower;

        public void OnClickReload() => _playerShooting.Reload();

        public void OnClickFreezeTime()
        {
            if(!_timeFreezer.FreezeTime()) return;
            StartCoroutine(FullImageCO(timeFreezerBtnImg, _timeFreezer.ReloadTime));
        }

        public void OnClickThrowGrenade()
        {
           if(!_grenadeThrower.SpawnGrenade()) return;
           StartCoroutine(FullImageCO(grenadeBtnImg, _grenadeThrower.GrenadeReloadTime));
        }

        private void Start()
        {
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _playerShooting = _playerMovement.GetComponent<PlayerShooting>();
            _grenadeThrower = _playerMovement.GetComponent<GrenadeThrower>();
            _timeFreezer = _playerMovement.GetComponent<TimeFreezer>();
        }

        private void Update()
        {
            if (!_inputEnapled) return;
            ShootingInput();
        }

        private void FixedUpdate()
        {
            if (!_inputEnapled) return;
            MovementInput();
        }

        private IEnumerator FullImageCO(Image image, float reloadTime)
        {
            var alpha = image.color.a;
            image.fillAmount = 0f;
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                emptyImageAlpha);
            while (image.fillAmount < 1f)
            {
                image.fillAmount += Time.deltaTime / reloadTime;
                yield return null;
            }

            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                alpha);
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