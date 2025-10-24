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

    void Update()
    {
        RotateTurretTowardsMouse();
    }
    
    private void RotateTurretTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        float angle = Mathf.Atan2(ray.direction.z, ray.direction.x) * Mathf.Rad2Deg;
        Quaternion lookRot = Quaternion.Euler(0, -angle + 180, 0);
        turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, lookRot, turretRotateSpeed * Time.deltaTime);
        
    }
}
