using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    
    [Header("References")] 
    public Transform playerTransform;
    public Vector3 offset;
    public float smoothSpeed = 1f;


    // void Update()
    // {
    //     if (Input.GetKey(KeyCode.LeftAlt))
    //     {
    //         
    //     }
    // }
    void LateUpdate()
    {
        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position =  smoothedPosition;
    }
}
