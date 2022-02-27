using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public enum State
    {
        MovingToTarget,
        AttackingTarget
    }
    [AddComponentMenu("Enemy/EnemyBehaviour")]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float distanceToAttack = 3f;
        [SerializeField] private float maxDistanceToDestroy = 200f;
        public static readonly UnityEvent OnEnemyTooFar = new UnityEvent();
        private IOnEnemyStateChange[] _components;
        private Transform _player;
        private float _sqrDistanceToAttack;
        private State _currentState = State.MovingToTarget;
        private State _previousState = State.MovingToTarget;
        
        private void Awake()
        {
            _components = GetComponents<IOnEnemyStateChange>();
        }
        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            _sqrDistanceToAttack = distanceToAttack * distanceToAttack;
        }
        private void Update()
        {
            CheckDistanceToDestroy();
            CheckDistanceToPlayer();
        }
        private void CheckDistanceToDestroy()
        {
            var distance = (transform.position - _player.position).magnitude;
            if (distance < maxDistanceToDestroy) return;
            OnEnemyTooFar?.Invoke();
            Destroy(gameObject);
        }
        
        private void CheckDistanceToPlayer()
        {
            var sqrDistance = (_player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance > _sqrDistanceToAttack)
            {
                _currentState = State.MovingToTarget;
            }
            else
            {
                _currentState = State.AttackingTarget;
            }
            if (_previousState == _currentState) return;
            ChangeState();
        }
        private void ChangeState()
        {
            _previousState = _currentState;
            foreach (var component in _components)
            {
                component.OnStateChange(_currentState);
            }
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward*distanceToAttack);
        }
    }
}