using Core;
using UnityEngine;

namespace Player
{
    [AddComponentMenu("Player/PlayerMovement")]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Joystick joystick;
        [SerializeField] private float speed = 15f;
        [Range(0, 1)]
        [SerializeField] private float movementStartValue = 0.2f;
        
        private Rigidbody _rigidBody;
        private bool _inputEnapled = true;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
        }
        private void OnEnable()
        {
            CustomEventHandler.OnInputPermissionChanged.AddListener(status => _inputEnapled = status);
            CommandHandler.OnPlayerSpeedChanged.AddListener(newSpeed => speed = newSpeed);
            CommandHandler.OnColliderVisibilityChanged.AddListener(mode => GetComponentInChildren<Collider>().enabled = mode);
        }
        private void Update()
        {
            if (!_inputEnapled) return;
            MovementInput();
        }
        private void MovementInput()
        {
            var xAxis = joystick.Horizontal;
            var zAxis = joystick.Vertical;
            var newForward = new Vector3(xAxis, 0, zAxis);
            if (newForward.magnitude > movementStartValue)
            {
                newForward.Normalize();
            }
            else
            {
                newForward = Vector3.zero;
            }
            var velocity = Vector3.ClampMagnitude(newForward * speed, speed);
            _rigidBody.velocity = velocity;
        }
    }
}
