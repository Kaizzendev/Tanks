using System;
using UnityEngine;

namespace Enemy
{
    public class ChaseController: EnemyControllerBase
    {
        
        [Header("Stats")]
        [SerializeField] private EnemyStats _enemyStats;
        
        internal void Chase(Vector3 position)
        {
            _enemyStats.navMeshAgent.isStopped = false;
            _enemyStats.navMeshAgent.SetDestination(position);
        }
    }
}