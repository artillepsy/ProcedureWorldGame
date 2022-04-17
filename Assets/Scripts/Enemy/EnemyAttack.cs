using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyAttack")]
    public class EnemyAttack : MonoBehaviour, IOnEnemyStateChange
    {
        [SerializeField] private float attackRateInSeconds;
        [SerializeField] private float damage;

        private PlayerHealth _playerHealth;
        private bool _attackMode = false;
        private float _totalAttackTime = 0;
        public UnityEvent OnKick = new UnityEvent();

        public void OnStateChange(State newEnemyState)
        {
            _attackMode = newEnemyState == State.AttackingTarget;
        }
        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
        }

        private void Update()
        {
            if (!ReadyToAttack()) return;
            if (!_attackMode) return;
            Attack();
        }

        private bool ReadyToAttack()
        {
            if (_totalAttackTime <= 0) return true;
            else _totalAttackTime -= Time.deltaTime;
            return false;
        }

        private void Attack()
        {
            _playerHealth.ChangeHealth(damage);
            _totalAttackTime = attackRateInSeconds;
            OnKick?.Invoke();
        }
    }
}