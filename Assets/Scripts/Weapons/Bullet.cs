using Enemy;
using UnityEngine;

namespace Weapons
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 10f;
        [SerializeField] private Rigidbody rb;
        
        private float _damage;
        private Vector3 _velocity;
        private Rigidbody _rb;
        private bool _instantiated = false;
        private void FixedUpdate()
        {
            if (!_instantiated) return;
            rb.velocity = _velocity;
        }
        public void SetUp(Vector3 direction, float damage, float speed)
        {
            transform.forward = direction;
            _damage = damage;
            _velocity = direction * speed;
            _instantiated = true;
            Destroy(gameObject, destroyDelay);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!_instantiated || other.isTrigger) return;
            var health = other.GetComponentInParent<EnemyHealth>();
            if (health)
            {
                health.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}
