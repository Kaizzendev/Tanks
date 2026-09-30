using System;
using System.Collections;
using Audio;
using DefaultNamespace;
using Objects;
using Player;
using Unity.Cinemachine;
using UnityEngine;

namespace Bomb
{
    public class Bomb: MonoBehaviour
    {
        public Action OnExplode;
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] internal float damage;
        [SerializeField] private GameObject explosion;
        [SerializeField] private CinemachineImpulseSource  _impulseSource;
        public EnumTeam team;
        
        [Header("Sounds")]
        [SerializeField] private AudioClip _placeBomb;
        [SerializeField] private AudioClip _bombExplosion;

        private void Start()
        {
            AudioManager.Instance.PlayOneShot2D(_placeBomb);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Enemy"))
            {
                Explode(other.transform);
            }
        }

        private void Explode(Transform transform)
        {
            Collider[] hit = Physics.OverlapSphere(transform.position, _sphereCollider.radius);
            
            foreach (Collider col in hit)
            {
                if (col.GetComponent<IDamageable>() != null)
                {
                    col.GetComponent<IDamageable>()?.TakeDamage(damage, team);
                }
            }
            
            Instantiate(explosion, transform.position, Quaternion.identity);
            _impulseSource.GenerateImpulse();
            OnExplode?.Invoke();
            
            AudioManager.Instance.PlayOneShot2D(_bombExplosion);
            
            GetComponent<MeshRenderer>().enabled = false;
            
            Destroy(gameObject, 1f);
        }
    }
}