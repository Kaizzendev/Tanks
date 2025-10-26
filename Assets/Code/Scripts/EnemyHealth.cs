using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    
    [Header("HP")]
    public int maxHealth = 100;
    public int currentHealth;
    
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
        Destroy(gameObject); //TODO: object pool
    }
}
