using System;
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
        [SerializeField] private float _dashDuration = 1000f;
        [SerializeField] private float _dashCooldown = 1000f;
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        internal void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                Dash();
            }
            
        }

        private void Dash()
        {
            Vector3 direction = _dashDirection.position - transform.position;
            direction.Normalize();
            _rb.AddForce(direction * _dashForce, ForceMode.Impulse);
        }
    }
}