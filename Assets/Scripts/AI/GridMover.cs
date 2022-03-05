using Player;
using UnityEngine;

namespace AI
{
    public class GridMover : MonoBehaviour
    {
        [SerializeField] private float moveDistance = 10f;
        [SerializeField] private float checkRateInSeconds = 1f;
        private Transform _player;
        void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            InvokeRepeating(nameof(UpdateSurfacePosition), 0, checkRateInSeconds);
        }
        private void UpdateSurfacePosition()
        {
            var distance = (transform.position - _player.position).magnitude;

            if (distance > moveDistance)
            {
                transform.position = _player.position;
            }
        }
    }
}
