using System;
using UnityEngine;

namespace Enemy
{
    public class PatrolController: EnemyControllerBase
    {
        [Header("Stats")]
        [SerializeField] private EnemyStats _enemyStats;
        
        private void Start()
        {
            _enemyStats.currentIndex = 0;
            _enemyStats.waiting = false;
        }

        public void Patrol()
        {
            if (_enemyStats.waiting)
            {
                _enemyStats.waitingTime += Time.deltaTime;
                if (_enemyStats.waitingTime >= _enemyStats.waitTimeAtPoint)
                {
                    _enemyStats.waiting = false;
                    MoveToNextPoint();
                }
            }

            if (!_enemyStats.navMeshAgent.pathPending && _enemyStats.navMeshAgent.remainingDistance < 0.5f)
            {
                StartWaiting();
            }
        }
        private void StartWaiting()
        {
            _enemyStats.waiting = true;
            _enemyStats.waitingTime = 0f;
            _enemyStats.navMeshAgent.isStopped = true;
        }

        private void MoveToNextPoint()
        {
            if (_enemyStats.patrolPoints.Length == 0) return;

            _enemyStats.navMeshAgent.isStopped = false;
            _enemyStats.navMeshAgent.SetDestination(_enemyStats.patrolPoints[_enemyStats.currentIndex].position);
            _enemyStats.currentIndex = (_enemyStats.currentIndex + 1) % _enemyStats.patrolPoints.Length; // recorre en bucle
        }
    }
}