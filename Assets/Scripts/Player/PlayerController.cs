using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Player
{
    public class PlayerController : PlayerControllerBase, IDamageable
    {
        
        [SerializeField] private MovementController _movementController;
        [SerializeField] private ShootController _shootController;
        [SerializeField] private BombController _bombController;
        [SerializeField] private DashController _dashController;

        [SerializeField] private float _invulnerabilityDuration;
        private float _invulnerabilityTimer;

        private float nextFireTime;
        
        private StateMachine _fsm;

        private void OnEnable()
        {
            GameEvents.onPlayerSpawn?.Invoke(transform);
        }

        private void Start()
        {
            _fsm = new StateMachine();

            _fsm.RegisterState(new AliveState(_fsm, this));
            _fsm.RegisterState(new DeadState(_fsm, this));
            _fsm.ChangeState<AliveState>();
        }

        private void Update()
        {
            _fsm?.Update();

            if (_fsm.GetCurrentState<AliveState>() != null)
            {
                return;
            }
                
            _movementController.isEnabled = false;
            _shootController.isEnabled = false;
            _bombController.isEnabled = false;
            _dashController.isEnabled = false;
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((other.CompareTag("Missile") && other.GetComponent<Missile>().team == EnumTeam.Enemy) && !IsInvulnerable())
            {
                TakeDamage(other.gameObject.GetComponent<Missile>().missileDamage);
            }
        }

        private bool IsInvulnerable()
        {
            bool isInvulnerable = Time.time < _invulnerabilityTimer + _invulnerabilityDuration;

            return isInvulnerable;
        }

        public void TakeDamage(float amount)
        {
            PlayerStats.Instance.currentHealth -= amount;
            _invulnerabilityTimer = Time.time;
            
            if (PlayerStats.Instance.currentHealth <= 0)
            {
                _fsm.ChangeState<DeadState>();
            }
        }
    }
}