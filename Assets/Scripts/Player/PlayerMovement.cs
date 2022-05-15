using UnityEngine;

namespace Player
{
    [AddComponentMenu("Player/PlayerMovement")]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 15f;
        private Rigidbody _rb;

        public void UpdateDirection(Vector3 direction) => _rb.velocity = direction.normalized * speed;
        
        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
    }
}
