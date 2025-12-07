using System;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("References")]
    public MeshRenderer wrap;
    public MeshRenderer wings;
    public Material color;
    
    [Header("Parameters")]
    public float missileSpeed = 20f;
    public float missileDamage = 100;
    
    private Rigidbody rb;
    private void Awake()
    {
        wrap.material = color;
        wings.material = color;
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction)
    {
        rb.linearVelocity = direction * missileSpeed;
    }
}
