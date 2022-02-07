using UnityEngine;

namespace Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform offsetPoint;
        [SerializeField] private float smoothTime = 0.25f;
        private Vector3 _offset;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _desiredPosition;
        private Transform _player;
        private void Awake()
        {
            _player = FindObjectOfType<Movement>().transform;
            _offset = transform.position - offsetPoint.position;
            _desiredPosition = offsetPoint.position;
        }
        private void FixedUpdate()
        {
            _desiredPosition = _player.position + _offset;
            var smoothedPosiiton = Vector3.SmoothDamp(transform.position, _desiredPosition, ref _velocity, smoothTime);
            transform.position = smoothedPosiiton;
        }
    }
}
