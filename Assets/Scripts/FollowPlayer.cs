using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [Header("References")] 
    public Transform playerTransform;
    
    [Header("Rotation")]
    public float rotationSpeed = 1f;
    
    float rotationInput;

    void Update()
    {
        rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            rotationInput = -1f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotationInput = 1f;
        }
    }
    void LateUpdate()
    {
        float cameraRotation = rotationInput * rotationSpeed * Time.deltaTime;
        if (rotationInput != 0)
        {
            transform.RotateAround(playerTransform.position, Vector3.up, cameraRotation);
        }
        
    }
}
