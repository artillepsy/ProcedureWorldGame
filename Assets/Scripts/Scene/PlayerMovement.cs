using Photon.Pun;
using UnityEngine;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] private float _speed = 15f;

    private string _horizontalInputAxis = Constants.InputAsixes.Horizontal;
    private string _verticalInputAxis = Constants.InputAsixes.Vertical;

    private Camera _camera;
    private Rigidbody _rigidBody;
    private Plane _rayCastPlane;
    private bool _inputEnapled = true;

    private void Awake()
    {
        if (!PhotonView.Get(this).IsMine)
        {
            enabled = false;
            return;
        }
        _rigidBody = GetComponent<Rigidbody>();
        _rayCastPlane = new Plane(Vector3.up, 0);

        CustomEventHandler.OnInputPermissionChanged.AddListener(status => _inputEnapled = status);
        CommandHandler.OnPlayerSpeedChanged.AddListener(newSpeed => _speed = newSpeed);
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
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (_rayCastPlane.Raycast(ray, out distance))
        {
            worldPosition = ray.GetPoint(distance);
            worldPosition.y = transform.position.y;

            Vector3 newForward = new Vector3(worldPosition.x - transform.position.x, 0, worldPosition.z - transform.position.z);
            transform.forward = newForward;
        }
    }
    private void MovementInput()
    {
        float xAxis = Input.GetAxisRaw(_horizontalInputAxis);
        float zAxis = Input.GetAxisRaw(_verticalInputAxis);
        Vector3 velocity = Vector3.ClampMagnitude(new Vector3(xAxis, 0, zAxis) * _speed, _speed);
        _rigidBody.velocity = velocity;
    }
}
