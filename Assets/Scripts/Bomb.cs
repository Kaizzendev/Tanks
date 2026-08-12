using System;
using UnityEngine;

namespace Bomb
{
    public class Bomb: MonoBehaviour
    {
        
        public Action OnExplode;
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] internal float damage;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                Explode();
            }
        }

        private void Explode()
        {
            OnExplode?.Invoke();
        }
    }
}