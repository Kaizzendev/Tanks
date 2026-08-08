using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretController : MonoBehaviour
{
    [Header("References")] 
    public Camera mainCamera;
    public Transform turretTransform;
    [Header("Rotation")]
    public float turretRotateSpeed = 150f;

    private void FixedUpdate()
    {
        RotateTurretTowardsMouse();
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void RotateTurretTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 mousePos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            mousePos = hit.point;
        }
        
        Vector3 direction = new Vector3(mousePos.x,0, mousePos.z) - new Vector3(turretTransform.position.x, 0, turretTransform.position.z);
        
        turretTransform.rotation = Quaternion.LookRotation(direction);
    }
    
}
