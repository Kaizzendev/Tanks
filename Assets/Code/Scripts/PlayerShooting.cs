using UnityEngine;

namespace Player
{

    public class PlayerShooting : MonoBehaviour
    {
        [Header("References")] public GameObject missile;
        public Transform spawnPoint;

        private float nextFireTime;


        private void Update()
        {
            if (Input.GetMouseButton(0) && Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + PlayerStats.Instance.attackSpeed;
            }
        }

        private void Shoot()
        {
            Vector3 direction = spawnPoint.position - transform.position;
            GameObject missilePrefab =
                Instantiate(missile, spawnPoint.position, spawnPoint.rotation); //TODO: Object pool
            missilePrefab.GetComponent<Missile>().missileDamage = PlayerStats.Instance.damage;
            missilePrefab.GetComponent<Missile>().Launch(direction);
        }
    }
}
