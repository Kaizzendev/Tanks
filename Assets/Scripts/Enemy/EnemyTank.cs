using System;
using System.Collections;
using Player;
using ProceduralGeneration;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyTank : MonoBehaviour
{
    public Transform player;

    [Header("Paramaters")] public float moveSpeed;
    public float rotateSpeed;
    public float detectionRange;
    public float fireRate;
    public float turretRotationSpeed;
    public float fireRange = 30f;

    public float fireTimer;

    [Header("References")]
    public Transform turret;
    public Transform firePoint;
    public GameObject missile;
    public NavMeshAgent navMeshAgent;
    private EnemyHealth _enemyHealth;
    
    [Header("Patrol system")]
    public Transform[] patrolPoints;
    public float waitTimeAtPoint = 1f;
    private int currentPatrolIndex;
    private bool waiting;
    
    private StateMachine fsm;

    public float distance;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = moveSpeed;
    }

    void Start()
    {
        ProceduralMap map = FindObjectOfType<ProceduralMap>();
        patrolPoints = shufflePatrolPoints(map.getPatrolPoints());

        
        
        fsm = new StateMachine();
        fsm.RegisterState(new PatrolState(fsm,this));
        fsm.RegisterState(new ChaseState(fsm, this));
        fsm.RegisterState(new AttackState(fsm, this));
        
        fsm.ChangeState<PatrolState>();
        
        
         if (player == null)
         {
             GameObject p = GameObject.FindGameObjectWithTag("Player");
             if (p != null) player = p.transform;
        }

        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private Transform[] shufflePatrolPoints(Transform[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            int randomIndex = Random.Range(i, points.Length);
            Transform p = points[randomIndex];
            points[i] = points[randomIndex];
            points[randomIndex] = p;
        }
        return points;
    }

    void Update()
    {
        distance = Vector3.Distance(player.position, transform.position);
        fsm.Update();
    }
    
    
}