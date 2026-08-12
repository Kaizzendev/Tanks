using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class DashController: PlayerControllerBase
    {
        
        private Rigidbody _rb;
        private float _moveInput;
        private float _rotationInput;
        [SerializeField] private Transform _dashDirection;
        
        [SerializeField] private float _dashForce = 1000f;
        [SerializeField] private float _dashDuration = 0.2f;
        [SerializeField] private float _dashCooldown = 1f;
        
        [SerializeField] private TrailRenderer[] _dashTrails;
        private float _dashStartTime;
        private float _lastTimeCasted;
        private bool _isDashing;
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        internal void Update()
        {
            if (!isEnabled)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > _lastTimeCasted + _dashCooldown && !_isDashing)
            {
                _isDashing =  true;
                _dashStartTime = Time.time;
            }

            if (_isDashing)
            {
                StartEmitter();
            }
            else
            {
                StopEmitter();
            }
            
        }

        private void FixedUpdate(){
            if (_isDashing)
            {
                if (Time.time < _dashStartTime + _dashDuration)
                {
                    Dash();
                }
                else
                {
                    _lastTimeCasted = Time.time;
                    _isDashing = false;
                }
            }
        }
        
        private void Dash()
        {
            Vector3 direction = _dashDirection.position - transform.position;
            direction.Normalize();
            _rb.linearVelocity = direction * _dashForce;
        }

        private void StartEmitter()
        {
            foreach (TrailRenderer trail in _dashTrails)
            {
                trail.emitting = true;
            }
        }

        private void StopEmitter()
        {
            foreach (TrailRenderer trail in _dashTrails)
            {
                trail.emitting = false;
            }
        }
    }
}