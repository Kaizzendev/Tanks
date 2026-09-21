using System;
using UnityEngine;

namespace Objects
{
    public class Destructible: MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private Material _mainMaterial;

        public void DestroyObject()
        {
            var main =  _particleSystem.main;
            main.startColor = _mainMaterial.color;
            Instantiate(_particleSystem, transform.position, Quaternion.identity);
            
            Destroy(gameObject);
        }
        
    }
}