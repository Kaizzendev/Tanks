using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _rb;
        private float _moveInput;
        private float _rotationInput;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            _moveInput = Input.GetAxis("Vertical");
            _rotationInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            MoveTank(_moveInput);
            RotateTank(_rotationInput);
        }

        private void RotateTank(float input)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, PlayerStats.Instance.rotationSpeed * input, 0));
            _rb.MoveRotation(_rb.rotation * rotation);
        }

        private void MoveTank(float input)
        {
            Vector3 move = input * transform.forward * PlayerStats.Instance.moveSpeed;
            _rb.linearVelocity = new Vector3(
                Mathf.Clamp(move.x, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed),
                _rb.linearVelocity.y,
                Mathf.Clamp(move.z, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed)
            );
        }
    }
}