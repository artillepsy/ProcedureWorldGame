using UnityEngine;

namespace Weapons
{
    public class Grenade : MonoBehaviour
    {
        [SerializeField] private float destroyDelay = 5f;
        [SerializeField] private float speed = 4f;
        [SerializeField] private ParticleSystem explosionPS;
        [SerializeField] private AudioSource src;
        
        private Rigidbody _rb;
        private bool _stopped = false;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            Destroy(gameObject, destroyDelay);
        }

        private void FixedUpdate()
        {
            if (_stopped) return;
            _rb.AddForce(transform.forward * speed);
        }

        private void OnCollisionEnter(Collision other)
        {
            explosionPS.Play();
            src.Play();
            _stopped = true;
        }
    }
}