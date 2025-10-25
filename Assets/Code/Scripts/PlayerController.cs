using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    public GameObject[] rightWheels;
    public GameObject[] leftWheels;

    [Header("Movement")] public float maxSpeed = 30f;
    public float moveSpeed = 20f;
    
    [Header("Rotation")]
    public float rotationSpeed = 2f;
    
    
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
        
        //RotateWheels(moveInput, rotationInput);
    }

    // private void RotateWheels(float f, float rotationInput1)
    // {
    //     float wheelRotation = moveInput * wheelRotationSpeed * Time.deltaTime;
    //
    //     foreach (var wheel in leftWheels)
    //     {
    //         wheel.transform.Rotate(0, wheelRotation -rotationInput * wheelRotationSpeed * Time.deltaTime, 0);
    //     }
    //
    //     foreach (var wheel in rightWheels)
    //     {
    //         wheel.transform.Rotate(0, wheelRotation + rotationInput * wheelRotationSpeed * Time.deltaTime, 0);
    //     }
    // }

    private void FixedUpdate()
    {
        MoveTank(moveInput);
        RotateTank(rotationInput);
    }

    private void RotateTank(float input)
    {
        
        Quaternion rotation = Quaternion.Euler(new Vector3(0, rotationSpeed * input, 0));
        rb.MoveRotation(rb.rotation * rotation);
    }

    private void MoveTank(float input)
    {
        Vector3 move = input * -transform.right * moveSpeed;
        rb.linearVelocity = new Vector3(
            Mathf.Clamp(move.x, -maxSpeed, maxSpeed),
            rb.linearVelocity.y,
            Mathf.Clamp(move.z, -maxSpeed, maxSpeed)
        );
    }
}
