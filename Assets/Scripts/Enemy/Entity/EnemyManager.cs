using System;
using Core;
using UnityEngine;

namespace Enemy.Entity
{
    [RequireComponent(typeof(EnemyDamageData))]
    [RequireComponent(typeof(EnemyHealthData))]
    [RequireComponent(typeof(EnemyMovementData))]
    public class EnemyManager : MonoBehaviour
    {
        private EnemyState _enemyState;
        private EnemyMovementData _enemyMovementData;
        private void Awake()
        { 
            _enemyState = EnemyState.MovingToTarget;
            _enemyMovementData = GetComponent<EnemyMovementData>();
        }

        private void Update()
        {
            // switch (_enemyState)
            // {
            //     case EnemyState.MovingToTarget:
            //         // moving to target
            //         
            // }
        }
    }
}