using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyAnimationController : MonoBehaviour, IOnEnemyStateChange
    {
        private Animator _animator;
        private static readonly int AttackMode = Animator.StringToHash("AttackMode");
        private static readonly int OnKick = Animator.StringToHash("OnKick");

        private void Start()
        {
            GetComponent<EnemyAttack>().OnKick.AddListener(OnKickEvent);
            _animator = GetComponentInChildren<Animator>();
        }
        public void OnStateChange(State newEnemyState)
        {
            _animator.SetBool(AttackMode ,newEnemyState == State.AttackingTarget);
            Debug.Log("Attack mode");
        }

        private void OnKickEvent()
        {
            _animator.SetTrigger(OnKick);
            Debug.Log("On Kick");
        }
    }
}