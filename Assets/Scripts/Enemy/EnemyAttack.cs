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
        
        private bool _isAttacking = false;
        private float _totalAttackTime = 0;
        

        public void OnStateChange(State newEnemyState)
        {
            _isAttacking = newEnemyState == State.AttackingTarget;
        }
        private void Start()
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
            
        }

        private void Update()
        {
            if (!ReadyToAttack()) return;
            if (!_isAttacking) return;
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
            _playerHealth.TakeDamage(damage);
            _totalAttackTime = attackRateInSeconds;
        }
    }
}