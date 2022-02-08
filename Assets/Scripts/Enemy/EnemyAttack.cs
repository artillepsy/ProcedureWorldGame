using System.Collections;
using Player;
using UnityEngine;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyAttack")]
    public class EnemyAttack : MonoBehaviour, IOnEnemyStateChange
    {
        [SerializeField] private float attackRateInSeconds;
        [SerializeField] private float damage;

        private PlayerHealth _playerHealth;
        private bool _isPlayerNear = false;
        private bool _isAttacking = false;

        public void OnStateChange(State newEnemyState)
        {
            _isAttacking = newEnemyState == State.AttackingTarget;
        }
        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            StartCoroutine(AttackingCoroutine());
        }
        private void Update()
        {
            if (_isPlayerNear)
            {
                transform.LookAt(_playerHealth.transform);
            }
        }
        private IEnumerator AttackingCoroutine()
        {
            while (true)
            {
                if (!_isPlayerNear && !_isAttacking)
                {
                    yield return null;
                    continue;
                }
                _playerHealth.TakeDamage(damage);
                yield return new WaitForSeconds(attackRateInSeconds);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<PlayerHealth>() == _playerHealth)
            {
                _isPlayerNear = true;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponentInParent<PlayerHealth>() == _playerHealth)
            {
                _isPlayerNear = false;
            }
        }
    }
}