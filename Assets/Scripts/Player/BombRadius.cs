using System;
using UnityEngine;

namespace Bomb
{
    public class BombRadius: MonoBehaviour
    {
        [SerializeField] private float _explosionRadius;
        [SerializeField] private Bomb _bomb;
        private bool hasExploded;
        private void OnEnable()
        {
            _bomb.OnExplode += CheckDamages;
        }

        private void OnDisable()
        {
            _bomb.OnExplode -= CheckDamages;
        }
        

        private void CheckDamages()
        {
            foreach (var go in Physics.OverlapSphere(transform.position, _explosionRadius))
            {
               //go.GetComponent<Player.PlayerStats>()?.TakeDamage(_bomb.damage);
                go.GetComponentInParent<EnemyHealth>()?.TakeDamage(_bomb.damage);
                Debug.Log(go.name);
            }
            Destroy(gameObject);
        }
    }
}