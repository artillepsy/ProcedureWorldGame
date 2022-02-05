using UnityEngine;

namespace Enemy.Entity
{
    public class EnemyDestinationSetter : MonoBehaviour
    {
        private EnemyMovementData _enemyMovementData;
        private Camera _camera;
        private Plane _plane;
        private bool _inputEnabled = true;

        void Awake()
        {
            _enemyMovementData = GetComponent<EnemyMovementData>();
            _camera = Camera.main;
            _plane = new Plane(Vector3.up, 0);

            CustomEventHandler.OnInputPermissionChanged.AddListener(status => _inputEnabled = status);
        }
        void Update()
        {
            MouseInput();
        }
        private void MouseInput()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0) || !_inputEnabled) return;

            float distance;
            Vector3 worldPosition;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance))
            {
                worldPosition = ray.GetPoint(distance); //+
                _enemyMovementData.SetDestination(worldPosition);
            }

        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5);
        }
    }
}
