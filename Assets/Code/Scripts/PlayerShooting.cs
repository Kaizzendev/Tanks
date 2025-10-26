using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("References")]
    public GameObject missile;
    public Transform spawnPoint;
    
    [Header("Settings")]
    public float fireRate = 0.5f;
    private float nextFireTime;
    

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            Shoot(); 
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        Vector3 direction = spawnPoint.position - transform.position;
        GameObject missilePrefab = Instantiate(missile, spawnPoint.position, spawnPoint.rotation); //TODO: Object pool
        missilePrefab.GetComponent<Missile>().Launch(direction);
    }
}
