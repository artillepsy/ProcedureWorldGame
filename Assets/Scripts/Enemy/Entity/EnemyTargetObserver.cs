using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Enemy.Entity
{
    public class EnemyTargetObserver : MonoBehaviour
    {
        [SerializeField] private float changeTargetRateInSeconds;
        private readonly List<Transform> _players = PlayerObserver.FoundPlayers;
        private Transform _target;
        private float _sqrMaxDistance;
        // check if player is dead (when i add health component to him)

        private void Start()
        {
            InvokeRepeating(nameof(SearchTarget), 0, changeTargetRateInSeconds);
        }
        private void SearchTarget()
        {
            var minDistance = float.MaxValue;
            foreach (var player in PlayerObserver.FoundPlayers)
            {
                var distance = Vector3.Distance(transform.position, player.position);
                if(distance >= minDistance) continue;
                minDistance = distance;
                _target = player;
            }
        }
        
    }
}