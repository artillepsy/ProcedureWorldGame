using System;
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
        [SerializeField] private float imgAlphaEmpty = 0.2f;
        private float _imgAlphaFull;
        private bool _inputEnapled = true;
        private PlayerMovement _playerMovement;
        private PlayerShooting _playerShooting;
        private GrenadeThrower _grenadeThrower;

        public void OnClickReload() => _playerShooting.Reload();
        public void OnClickThrowGrenade()
        {
           if(!_grenadeThrower.SpawnGrenade()) return;
           StartCoroutine(FullGrenadeImgCO());
        }

        private void Start()
        {
            _imgAlphaFull = grenadeBtnImg.color.a;
            _playerMovement = FindObjectOfType<PlayerMovement>();
            _playerShooting = FindObjectOfType<PlayerShooting>();
            _grenadeThrower = FindObjectOfType<GrenadeThrower>();
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

        private IEnumerator FullGrenadeImgCO()
        {
            var reloadTime = _grenadeThrower.GrenadeReloadTime;
            grenadeBtnImg.fillAmount = 0f;
            grenadeBtnImg.color = new Color(
                grenadeBtnImg.color.r,
                grenadeBtnImg.color.g,
                grenadeBtnImg.color.b,
                imgAlphaEmpty);
            while (grenadeBtnImg.fillAmount < 1f)
            {
                grenadeBtnImg.fillAmount += Time.deltaTime / reloadTime;
                yield return null;
            }

            grenadeBtnImg.color = new Color(
                grenadeBtnImg.color.r,
                grenadeBtnImg.color.g,
                grenadeBtnImg.color.b,
                _imgAlphaFull);
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