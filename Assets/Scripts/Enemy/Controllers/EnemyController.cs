using System;
using System.Collections.Generic;
using Audio;
using DefaultNamespace;
using Enemy.UtilityAI;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyController: EnemyControllerBase, IDamageable
    {
        [Header("Debug")] [SerializeField] private bool _isDebug;
        
        [Header("Controllers")]
        [SerializeField] internal PatrolController _patrolController;
        [SerializeField] internal ChaseController _chaseController;
        [SerializeField] internal AttackController _attackController;
        
        [Header("Enemy Stats")]
        [SerializeField] private EnemyStats _stats;

        [Header("References")] [SerializeField]
        private Material _mainMaterial;
        public GameObject explosion;
        
        [Header("Sounds")]
        [SerializeField] internal AudioSource _movingSound;
        [SerializeField] internal AudioClip _explosionSound;
        
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

        private void Start()
        {
            _fsm = new StateMachine();
            _fsm.RegisterState(new AliveState(_fsm, this, _stats));
            _fsm.RegisterState(new DeadState(_fsm, this, _stats));

            _stats.navMeshAgent.speed = _stats.moveSpeed;
            
            _stats.patrolPoints.Add(transform.position);
            _stats.currentHealth = _stats.maxHealth;
        }

        public void Attack()
        {
            _attackController.Attack(_player.position);
        }

        public void Chase()
        {
            _chaseController.Chase(_player.position);
        }

        public void Patrol()
        {
            _patrolController.Patrol();
        }


        private void Update()
        {
            if (_player != null)
            {
                distance = Vector3.Distance(_player.position, transform.position);
            }
            
            _fsm.Update();
            
            if (_stats.navMeshAgent.velocity != Vector3.zero)
            {
                if (!_movingSound.isPlaying)
                {
                    _movingSound.Play();
                }
            }
            else
            {
                _movingSound.Stop();
            }
            
        }

        internal void SwitchGameplay(bool isEnabled)
        {
            _patrolController.isEnabled = isEnabled;
            _chaseController.isEnabled = isEnabled;
            _attackController.isEnabled = isEnabled;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if ((other.CompareTag("Missile") && other.GetComponent<Missile>().team == EnumTeam.Player))
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
            if (_fsm.GetCurrentState<DeadState>() != null)
            {
                return;
            }

            if (IsInvulnerable())
            {
                return;
            }
            
            _stats.currentHealth -= amount;
            _stats._invulnerabilityTimer = Time.time;
            if (_stats.currentHealth <= 0)
            {
                Debug.Log($"DEAD");
                Die();
            }
        }

        public void TakeDamage(float amount, EnumTeam team)
        {
            if (_fsm.GetCurrentState<DeadState>() != null)
            {
                return;
            }

            if (IsInvulnerable())
            {
                return;
            }

            if (team != EnumTeam.Player)
            {
                return;
            }
            
            _stats.currentHealth -= amount;
            _stats._invulnerabilityTimer = Time.time;
            if (_stats.currentHealth <= 0)
            {
                Debug.Log($"DEAD");
                Die();
            }
            
        }

        private void Die()
        {
            AssignColor();
            Instantiate(explosion, transform.position, Quaternion.identity);
            
            EnemyEvents.OnEnemyDied?.Invoke();
            _fsm.ChangeState<DeadState>();
            
            AudioManager.Instance.PlayOneShot2D(_explosionSound);
        }

        private void AssignColor()
        {
            Color color = _mainMaterial.color;

            ParticleSystem particleSystem = explosion.transform.GetChild(0).GetComponent<ParticleSystem>();
            var main = particleSystem.main;

            main.startColor = color;
        }

        private void OnDrawGizmos()
        {
            if (_isDebug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position, _stats.fireRange);
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, _stats.detectionRange);
            }
        }
    }
}