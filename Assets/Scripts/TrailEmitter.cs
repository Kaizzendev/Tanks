using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class TrailEmitter: MonoBehaviour
    {
        
        [SerializeField] private TrailRenderer[] _trailRenderers;
        private void Update()
        {
            StartEmitter();
        }

        private void StartEmitter()
        {
            foreach (TrailRenderer trail in _trailRenderers)
            {
                trail.emitting = true;
            }
        }

        private void StopEmitter()
        {
            foreach (TrailRenderer trail in _trailRenderers)
            {
                trail.emitting = false;
            }
        }

        private void OnDestroy()
        {
            StopEmitter();
        }
    }
}