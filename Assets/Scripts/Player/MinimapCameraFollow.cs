using UnityEngine;

namespace Player
{
    public class MinimapCameraFollow : MonoBehaviour
    {
        [SerializeField] private float size = 50;
        private Transform _player;
        private void Awake()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            GetComponent<Camera>().orthographicSize = size;
        }
        private void FixedUpdate()
        {
            transform.position  =  new Vector3(_player.position.x, transform.position.y, _player.position.z);
        }
    }
}
