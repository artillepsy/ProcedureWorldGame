using Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyMovement")]
    public class EnemyMovement : MonoBehaviour, IOnEnemyStateChange
    {
        [SerializeField] private float angularSpeed = 60f;
        [SerializeField] private float repathRateInSeconds = 0.2f;
        [SerializeField] private float stopRotationAngle = 1f;
        
        private NavMeshAgent _agent;
        private Transform _player;
        private float _totalRepathTime = 0;
        private bool _isMoving = true;
        private float _angularSpeed;

        public void OnStateChange(State newEnemyState)
        {
            _isMoving = newEnemyState == State.MovingToTarget;
            if (!_isMoving) _agent.ResetPath();
        }
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }
        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>().transform;
        }
        
        private void Update()
        {
            if(!_isMoving) RotateToTarget();
            else if(ReadyToUpdate()) UpdatePath();
        }

        private bool ReadyToUpdate()
        {
            if (_totalRepathTime > 0)
            {
                _totalRepathTime -= Time.deltaTime;
                return false;
            }
            return true;
        }
        
        private void UpdatePath()
        {
            if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
            {
                _agent.SetDestination(_player.position);
            }
            _totalRepathTime = repathRateInSeconds;
        }

        private void RotateToTarget()
        {
            var directionToTarget = (_player.position - transform.position).normalized;
            directionToTarget.y = 0f;
            var rotationToTarget = Quaternion.LookRotation(directionToTarget);
            
            if (Quaternion.Angle(transform.rotation, rotationToTarget) < stopRotationAngle) return;
            var rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget,
                angularSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
}
