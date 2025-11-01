using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [Header("HP")]
    public int maxHealth = 100;
    public int currentHealth;
    
    public bool isDead;
    [Header("References")]
    public GameObject explosion;
    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile") || other.CompareTag("Bomb"))
        {
            currentHealth -= other.GetComponent<Missile>().missileDamage;
            if (currentHealth <= 0)
            {
               
                Death();
            }
        }
    }

    private void Death()
    {
        isDead = true;
        Instantiate(explosion,transform.position,Quaternion.identity);
        Destroy(gameObject); //TODO: object pool
    }
}
