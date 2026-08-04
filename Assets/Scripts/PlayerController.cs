using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody rb;
        private float moveInput;
        private float rotationInput;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            moveInput = Input.GetAxis("Vertical");
            rotationInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            MoveTank(moveInput);
            RotateTank(rotationInput);
        }

        private void RotateTank(float input)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, PlayerStats.Instance.rotationSpeed * input, 0));
            rb.MoveRotation(rb.rotation * rotation);
        }

        private void MoveTank(float input)
        {
            Vector3 move = input * -transform.right * PlayerStats.Instance.moveSpeed;
            rb.linearVelocity = new Vector3(
                Mathf.Clamp(move.x, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed),
                rb.linearVelocity.y,
                Mathf.Clamp(move.z, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed)
            );
        }
    }
}