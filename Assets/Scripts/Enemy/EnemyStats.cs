using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyStats: MonoBehaviour
    {
        [Header("Health Stats")]
        [SerializeField] internal float maxHealth = 100f;
        [SerializeField] internal float currentHealth;

        [Header("Damage Stats")]
        [SerializeField] internal float damage = 100f;
        [SerializeField] internal float attackSpeed = 1f;
        [SerializeField] internal float criticChance = 0f;
        [SerializeField] internal float criticMultiplier = 2f;
        [SerializeField] internal float fireTimer;
        [SerializeField] internal float fireRate = 2;
        [SerializeField] internal float fireRange = 25f;
        
        [Header("Movement Stats")]
        [SerializeField] internal float maxMoveSpeed = 14f;
        [SerializeField] internal float moveSpeed = 6f;
        [SerializeField] internal float turretRotationSpeed = 2;

        [Header("Navigation")]
        [SerializeField] internal Transform[] patrolPoints;
        [SerializeField] internal float detectionRange = 40f;
        [SerializeField] internal int currentIndex = 0;
        [SerializeField] internal bool waiting;
        [SerializeField] internal float waitingTime;
        [SerializeField] internal float waitTimeAtPoint = 1f;
        
        [Header("Invulnerability")]
        [SerializeField] internal float _invulnerabilityDuration;
        internal float _invulnerabilityTimer;
        
        [Header("References")]
         [SerializeField] internal Transform turret;
         [SerializeField] internal Transform firePoint;
         [SerializeField] internal GameObject missile;
         [SerializeField] internal NavMeshAgent navMeshAgent;
    }
}