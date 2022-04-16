using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Enemy
{
    public enum State
    {
        MovingToTarget,
        AttackingTarget,
        Dying
    }
    [AddComponentMenu("Enemy/EnemyBehaviour")]
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private float distanceToAttack = 3f;
        [SerializeField] private float maxDistanceToDestroy = 200f;
        [SerializeField] private float timeAfterDeadToDestroy = 4f;
        
        public static readonly UnityEvent OnEnemyTooFar = new UnityEvent();
        private IOnEnemyStateChange[] _components;
        private Transform _player;
        private float _sqrDistanceToAttack;
        private State _currentState = State.MovingToTarget;
        private State _previousState = State.MovingToTarget;
        private bool _dead = false;
        
        private void Awake()
        {
            _components = GetComponents<IOnEnemyStateChange>();
        }
        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
            _sqrDistanceToAttack = distanceToAttack * distanceToAttack;
            GetComponent<EnemyHealth>().OnDie?.AddListener(() =>
            {
                ChangeState(State.Dying);
                _dead = true;
                Destroy(gameObject, timeAfterDeadToDestroy);
            });
        }
        private void Update()
        {
            if (_dead) return;
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

        private void ChangeState(State newState)
        {
            _currentState = newState;
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