using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")] 
    public GameObject[] rightWheels;
    public GameObject[] leftWheels;
    
    [Header("Movement")]
    public float moveSpeed = 20f;
    public float wheelRotationSpeed = 200f;
    
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

    private void RotateWheels(float f, float rotationInput1)
    {
        float wheelRotation = moveInput * wheelRotationSpeed * Time.deltaTime;

        foreach (var wheel in leftWheels)
        {
            wheel.transform.Rotate(0, wheelRotation -rotationInput * wheelRotationSpeed * Time.deltaTime, 0);
        }

        foreach (var wheel in rightWheels)
        {
            wheel.transform.Rotate(0, wheelRotation + rotationInput * wheelRotationSpeed * Time.deltaTime, 0);
        }
    }

    private void FixedUpdate()
    {
        MoveTank(moveInput);
        RotateTank(rotationInput);
    }

    private void RotateTank(float input)
    {
        throw new NotImplementedException();
    }

    private void MoveTank(float input)
    {
        Vector3 move = input * transform.right * moveSpeed * Time.fixedDeltaTime;
        rb.AddForce(move,ForceMode.Acceleration);
    }
}
