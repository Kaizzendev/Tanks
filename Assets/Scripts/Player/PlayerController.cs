using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : PlayerControllerBase
    {
        
        [SerializeField] private MovementController _movementController;
        [SerializeField] private ShootController _shootController;
        [SerializeField] private BombController _bombController;
        [SerializeField] private DashController _dashController;

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

            if (_fsm.GetCurrentState<AliveState>() == null)
            {
                _movementController.isEnabled = false;
                _shootController.isEnabled = false;
                _bombController.isEnabled = false;
                _dashController.isEnabled = false;
            }
        }

        private void FixedUpdate()
        {
            _fsm?.FixedUpdate();
        }
        
    }
}