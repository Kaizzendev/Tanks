using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TurretController : MonoBehaviour
{
    [Header("References")] 
    public Camera mainCamera;
    public Transform turretTransform;
    public Transform canon;
    [Header("Rotation")]
    public float turretRotateSpeed = 150f;
    public float canonRotateSpeed = 5f;
    public float minElevation = 0;
    public float maxElevation = 50f;

    private void FixedUpdate()
    {
        RotateTurretTowardsMouse();
    }
    
    private void RotateTurretTowardsMouse()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        float angle = Mathf.Atan2(ray.direction.z, ray.direction.x) * Mathf.Rad2Deg;
        Quaternion lookRot = Quaternion.Euler(0, -angle + 180, 0);
        turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, lookRot, turretRotateSpeed * Time.deltaTime);
        
        
        float cannonAngle = Mathf.Atan2(ray.direction.y, ray.direction.x) * Mathf.Rad2Deg;
        cannonAngle = Mathf.Clamp(cannonAngle, minElevation, maxElevation);

        Quaternion canonRot = Quaternion.Euler(0f,0f, -cannonAngle * 2);
        canon.localRotation = Quaternion.Slerp(
            canon.localRotation,
            canonRot,
            canonRotateSpeed * Time.deltaTime
        );
        
    }
    
}
