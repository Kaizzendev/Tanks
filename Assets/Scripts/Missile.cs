using System;
using System.Collections;
using Audio;
using DefaultNamespace;
using Objects;
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

    [Header("Sounds")] 
    [SerializeField] private AudioClip _ricochet;

    [SerializeField] private AudioClip _shoot;

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
        AudioManager.Instance.PlayOneShot2D(_shoot);
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
            other.GetComponent<Destructible>().DestroyObject();
        }
        Destroy(gameObject);
    }
}
