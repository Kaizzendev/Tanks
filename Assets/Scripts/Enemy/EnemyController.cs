using System;
using DefaultNamespace;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyController: EnemyControllerBase, IDamageable
    {
        [Header("Controllers")]
        [SerializeField] internal PatrolController _patrolController;
        [SerializeField] internal ChaseController _chaseController;
        [SerializeField] internal AttackController _attackController;
        
        [Header("Enemy Stats")]
        [SerializeField] private EnemyStats _stats;
        
        [Header("References")]
        public GameObject explosion;
        
        internal float distance;
        
        private StateMachine _fsm;
        
        internal Transform _player;

        private void OnEnable()
        {
           EnemyEvents.OnEnemySpawned?.Invoke();
           GameEvents.onPlayerSpawn += RegisterPlayer;
        }

        private void OnDisable()
        {
            GameEvents.onPlayerSpawn -= RegisterPlayer;
        }

        private void RegisterPlayer(Transform player)
        {
            _player = player;
            _fsm.ChangeState<AliveState>();
        }

        private void Start() // Alñgo falla que no se desplaza hacia el jugador
        {
            _fsm = new StateMachine();
            _fsm.RegisterState(new AliveState(_fsm, this, _stats));
            _fsm.RegisterState(new DeadState(_fsm, this));

            _stats.navMeshAgent.speed = _stats.moveSpeed;
        }


        private void Update()
        {
            if (_player != null)
            {
                distance = Vector3.Distance(_player.position, transform.position);
            }
            
            _fsm.Update();
        }

        internal void SwitchGameplay(bool isEnabled)
        {
            _patrolController.isEnabled = isEnabled;
            _chaseController.isEnabled = isEnabled;
            _attackController.isEnabled = isEnabled;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((other.CompareTag("Missile") && other.GetComponent<Missile>().team == EnumTeam.Player) && !IsInvulnerable())
            {
                TakeDamage(other.gameObject.GetComponent<Missile>().missileDamage);
            }
        }

        private bool IsInvulnerable()
        {
            bool isInvulnerable = Time.time < _stats._invulnerabilityTimer + _stats._invulnerabilityDuration;

            return isInvulnerable;
        }
        
        
        public void TakeDamage(float amount)
        {
            EnemyEvents.OnEnemyDied?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _stats.fireRange);
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _stats.detectionRange);
        }
    }
}