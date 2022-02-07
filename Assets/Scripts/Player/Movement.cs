using Core;
using UnityEngine;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;

        private string _horizontalInputAxis = Constants.InputAsixes.Horizontal;
        private string _verticalInputAxis = Constants.InputAsixes.Vertical;

        private Camera _camera;
        private Rigidbody _rigidBody;
        private Plane _rayCastPlane;
        private bool _inputEnapled = true;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _rayCastPlane = new Plane(Vector3.up, 0);
        }
        private void OnEnable()
        {
            CustomEventHandler.OnInputPermissionChanged.AddListener(status => _inputEnapled = status);
            CommandHandler.OnPlayerSpeedChanged.AddListener(newSpeed => speed = newSpeed);
            CommandHandler.OnColliderVisibilityChanged.AddListener(mode => GetComponentInChildren<Collider>().enabled = mode);
        }
        private void Start()
        {
            _camera = Camera.main;
        }
        private void Update()
        {
            if (_inputEnapled)
            {
                MovementInput();
                MouseInput();
            }
        }
        private void MouseInput()
        {
            float distance;
            Vector3 worldPosition;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_rayCastPlane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance);
                worldPosition.y = transform.position.y;

                var newForward = new Vector3(worldPosition.x - transform.position.x, 0, worldPosition.z - transform.position.z);
                transform.forward = newForward;
            }
        }
        private void MovementInput()
        {
            var xAxis = Input.GetAxisRaw(_horizontalInputAxis);
            var zAxis = Input.GetAxisRaw(_verticalInputAxis);
            var velocity = Vector3.ClampMagnitude(new Vector3(xAxis, 0, zAxis) * speed, speed);
            _rigidBody.velocity = velocity;
        }
    }
}
