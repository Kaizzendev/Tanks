using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Manager;
using UnityEngine;

namespace Player
{
    public class PlayerController : PlayerControllerBase, IDamageable
    {
        [Header("Controllers")]   
        [SerializeField] private MovementController _movementController;
        [SerializeField] private ShootController _shootController;
        [SerializeField] private BombController _bombController;
        [SerializeField] private DashController _dashController;
        [SerializeField] private MapNavigationController _navigationController;

        [Header("Invulnerability")]
        [SerializeField] private float _invulnerabilityDuration;
        private float _invulnerabilityTimer;

        private float nextFireTime;
        
        private StateMachine _fsm;

        private void OnEnable()
        {
            GameManager.Instance.OnStateChanged += OnStateChanged;
            GameEvents.onPlayerSpawn?.Invoke(transform);
        }

        private void OnDisable()
        {
            GameManager.Instance.OnStateChanged -= OnStateChanged;
        }

        private void OnStateChanged(GameManager.GameState gameState)
        {
            if (_fsm == null)
            {
                StartStateMachine();
            }
            switch (gameState)
            {
                case GameManager.GameState.MapNavigation:
                    _fsm.ChangeState<MapNavigationState>();
                    break;
                case GameManager.GameState.GameOver:
                    _fsm.ChangeState<DeadState>();
                    break;
                case GameManager.GameState.Playing:
                    _fsm.ChangeState<AliveState>();
                    break;
            }
        }

        private void Start()
        {
            if (_fsm == null)
            {
                StartStateMachine();
            }

            OnStateChanged(GameManager.Instance.CurrentState);
        }

        private void StartStateMachine()
        {
            _fsm = new StateMachine();

            _fsm.RegisterState(new AliveState(_fsm, this));
            _fsm.RegisterState(new DeadState(_fsm, this));
            _fsm.RegisterState(new MapNavigationState(_fsm, this));
        }

        internal void SwitchGameplay(bool isEnabled)
        {
            _movementController.isEnabled = isEnabled;
            _shootController.isEnabled = isEnabled;
            _bombController.isEnabled = isEnabled;
            _dashController.isEnabled = isEnabled;
        }

        internal void SwitchMapNavigation(bool isEnabled)
        {
            _navigationController.isEnabled = isEnabled;
        }

        private void Update()
        {
            _fsm?.Update();
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