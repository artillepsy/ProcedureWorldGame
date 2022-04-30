using Player;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

namespace Enemy
{
    [AddComponentMenu("Enemy/EnemyMovement")]
    public class EnemyMovement : MonoBehaviour, IOnEnemyStateChange
    {
        [SerializeField] private float stopTimeWhenDamaged = 0.1f;
        [SerializeField] private float angularSpeed = 60f;
        [SerializeField] private float repathRateInSeconds = 0.2f;
        [SerializeField] private float stopRotationAngle = 1f;
        
        private NavMeshAgent _agent;
        private Transform _player;
        private Transform _target;
        private float _totalRepathTime = 0;
        private bool _isMoving = true;
        private float _angularSpeed;
        private float _agentSpeed; 

        public void OnStateChange(State newEnemyState)
        {
            _isMoving = newEnemyState == State.MovingToTarget;
            if (!_isMoving) _agent.ResetPath();
        }

        private void OnEnable()
        {
            GetComponent<EnemyHealth>().OnTakeDmg.AddListener(() =>
            {
                _agent.speed = 0f;
                CancelInvoke(nameof(RevertSpeed));
                Invoke(nameof(RevertSpeed), stopTimeWhenDamaged);
            });
        }

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agentSpeed = _agent.speed;
            _player = FindObjectOfType<PlayerMovement>().transform;
            InvokeRepeating(nameof(UpdateTarget), 0, 1);
        }
        
        private void Update()
        {
            if(!_target) UpdateTarget();
            if(!_isMoving) RotateToTarget();
            else if(ReadyToUpdate()) UpdatePath();
        }

        private void RevertSpeed() => _agent.speed = _agentSpeed;

        private void UpdateTarget()
        {
            var grenade = FindObjectOfType<Grenade>();
            _target = grenade ? grenade.transform : _player;
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
                _agent.SetDestination(_target.position);
                
            }
            _totalRepathTime = repathRateInSeconds;
        }

        private void RotateToTarget()
        {
            var directionToTarget = (_target.position - transform.position).normalized;
            directionToTarget.y = 0f;
            var rotationToTarget = Quaternion.LookRotation(directionToTarget);
            
            if (Quaternion.Angle(transform.rotation, rotationToTarget) < stopRotationAngle) return;
            var rotation = Quaternion.RotateTowards(transform.rotation, rotationToTarget,
                angularSpeed * Time.deltaTime);
            transform.rotation = rotation;
        }
    }
}
