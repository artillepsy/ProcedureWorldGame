using UnityEngine;

namespace Core
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cam;
        private void Start() => _cam = Camera.main.transform;
        private void Update()
        {
            var pos = _cam.position;
            pos.x = transform.position.x;
            var worldUp = _cam.position.z > transform.position.z ? Vector3.down : Vector3.up;
            transform.LookAt(pos, worldUp);
        }
    }
}