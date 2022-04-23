using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator _animator;
        private Rigidbody _rb;
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }
        
        private void Update()
        {
            var angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
            var velocity = Quaternion.Euler(0, -angle, 0) *_rb.velocity;
            _animator.SetFloat(VelocityX, velocity.x);
            _animator.SetFloat(VelocityZ, velocity.z );
        }
    }
}
