using System;
using Player;
using Unity.Cinemachine;
using UnityEngine;
using Object = System.Object;

namespace CameraNamespace
{
    public class PlayerCameraBrain : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera playerCamera;

        [Header("Camera config")]
        [SerializeField] private Vector3 cameraOffsetPosition = new Vector3(0,40,-10);
        [SerializeField] private Vector2 cameraDeadZone = new Vector2(0.4f,0.4f);
        [SerializeField] private Vector2 cameraHardLimits = new Vector2(0.6f,0.6f);
        [SerializeField] private float cameraDistance = 30f;
        [SerializeField] [Range(0f,1f)] private float cameraLookAheadTime = 1f;
        [SerializeField] [Range(0f,30f)] private float cameraLookAheadSmoothing = 10f;
        private void OnEnable()
        {
            GameEvents.onPlayerSpawn += SetTarget;
        }

        public void SetTarget(Transform target)
        {
            playerCamera.Target.TrackingTarget = target;
            
            ConfigCamera();
        }

        private void ConfigCamera()
        {
            CinemachinePositionComposer composer = playerCamera.gameObject.GetComponent<CinemachinePositionComposer>();
            
            if (composer == null)
            {
                composer = playerCamera.gameObject.AddComponent<CinemachinePositionComposer>();
            }

            composer.CameraDistance = cameraDistance;

            composer.Composition.DeadZone.Enabled = true;
            composer.Composition.DeadZone.Size = cameraDeadZone;
            composer.Lookahead.Enabled = true;
            composer.Lookahead.Smoothing = cameraLookAheadSmoothing;
            composer.Lookahead.Time = cameraLookAheadTime;
            composer.Composition.HardLimits.Enabled = true;
            composer.Composition.HardLimits.Size = cameraHardLimits;

        }

        private void OnDisable()
        {
            GameEvents.onPlayerSpawn -= SetTarget;
        }
    }
}
