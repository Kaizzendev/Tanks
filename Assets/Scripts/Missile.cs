using System;
using System.Collections;
using DefaultNamespace;
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
    public float timeToDespawn = 10f;


    public EnumTeam team;
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

    private void Start()
    {
        Destroy(gameObject, timeToDespawn);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }
        Destroy(gameObject);
    }
}
