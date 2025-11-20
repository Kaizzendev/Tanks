using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTank : MonoBehaviour
{
    public Transform player;

    [Header("Paramaters")] public float moveSpeed;
    public float rotateSpeed;
    public float detectionRange;
    public float fireRange;
    public float fireRate;
    public float turretRotationSpeed;

    private float fireTimer;

    [Header("References")] public Transform turret;
    public Transform firePoint;
    public GameObject missile;
    public NavMeshAgent navMeshAgent;
    private EnemyHealth _enemyHealth;

    
    [Header("Patrol system")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 1f;
    private int currentPatrolIndex;
    private bool waiting;
    private enum State
    {
        Idle,
        Chase,
        Attack,
        Dead,
        Patrol
    }

    private State currentState = State.Idle;


    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Start()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        _enemyHealth = GetComponent<EnemyHealth>();
        
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
        else
        {
            currentState = State.Idle;
        }
    }

    void Update()
    {
        if (_enemyHealth == null || _enemyHealth.isDead) return;

        float distance = Vector3.Distance(transform.position, player.position);
        print(currentState);
        switch (currentState)
        {
            case State.Idle:
                if (distance < detectionRange)
                {
                    currentState = State.Chase;
                }

                break;

            case State.Patrol:
                PatrolBehavior();
                if (distance < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                ChaseBehavior();
                RotateTowards(player.position);
                if (distance < fireRange)
                {
                    currentState = State.Attack;
                }

                if (distance > detectionRange)
                {
                    currentState = State.Patrol;
                }

                break;

            case State.Attack:
                RotateTurretTowards(player.position);
                Shoot();
                if (distance > fireRange * 1.2f)
                {
                    currentState = State.Chase;
                }

                break;
        }
    }

    private void PatrolBehavior()
    {
        if (waiting || patrolPoints == null || patrolPoints.Length == 0)
            return;
        
        if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
            StartCoroutine(WaitAndGoNextPatrol());
    }
    
    IEnumerator WaitAndGoNextPatrol()
    {
        waiting = true;
        navMeshAgent.isStopped = true;
        yield return new WaitForSeconds(waitTimeAtPoint);
        waiting = false;
        GoToNextPatrolPoint();
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(patrolPoints[currentPatrolIndex].position);
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length; // recorre en bucle
    }

    private void ChaseBehavior()
    {
        navMeshAgent.SetDestination(player.position);
    }

    private void RotateTowards(Vector3 target)
    {
        Vector3 dir = (target - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        lookRot *= Quaternion.Euler(0, 90, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * rotateSpeed);
    }

    private void RotateTurretTowards(Vector3 target)
    {
        Vector3 dir = (target - turret.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        lookRot *= Quaternion.Euler(0, 90, 0);
        turret.rotation = Quaternion.Lerp(turret.rotation, lookRot, Time.deltaTime * turretRotationSpeed);
    }

    private void Shoot()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0;
            Vector3 direction = firePoint.position - transform.position;
            GameObject missilePrefab = Instantiate(missile, firePoint.position, firePoint.rotation);
            missilePrefab.GetComponent<Missile>().Launch(direction);
            missilePrefab.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}