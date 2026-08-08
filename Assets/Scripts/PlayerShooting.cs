using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
            Vector3 direction = new Vector3(spawnPoint.position.x,4.2f,spawnPoint.position.z) - new Vector3(transform.position.x, 4.2f, transform.position.z);
            GameObject missilePrefab =
                Instantiate(missile, spawnPoint.position, spawnPoint.rotation * Quaternion.Euler(1,-90,1)); //TODO: Object pool
            missilePrefab.GetComponent<Missile>().missileDamage = SetDamage();
            missilePrefab.GetComponent<Missile>().Launch(direction);
        }

        private float SetDamage()
        {
            float damage = PlayerStats.Instance.damage;
            if (Random.Range(0, 100) < PlayerStats.Instance.criticChance)
            {
                damage *= PlayerStats.Instance.criticMultiplier;
            }
            print("Disparo con daño de: " + damage);
            return damage;
        }
    }
}
