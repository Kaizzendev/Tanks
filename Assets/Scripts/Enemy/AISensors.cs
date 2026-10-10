using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class AISensors: MonoBehaviour
    {
        [Header("FOV configuration")]
        [SerializeField] private float _distance;
        [SerializeField] private float _angle;
        [SerializeField] private Material _meshMaterial;
        [SerializeField][Range(50,120)] private int _visionConeResolution = 120;

        [Header("Layer Mask")]
        [SerializeField] private LayerMask _obstructingVisionLayerMask;
        [SerializeField] private LayerMask _playerLayerMask;

        private MeshFilter _meshFilter;
        private Mesh _visionConeMesh;

        private GameObject _visionConeVisual;
        private Vector3 _randomDirection;
        
        private float _angleInRadians;
        private bool _isPlayerInDetectionRange;
        
        [Header("Stats")]
        [SerializeField] private EnemyStats _enemyStats;

        [SerializeField] private EnemyController _enemyController;
        
        private void Start()
        {
            _visionConeVisual = new GameObject("visionConeVisual");
            
            _visionConeVisual.transform.SetParent(_enemyStats.firePoint, false);
            
            _visionConeVisual.AddComponent<MeshRenderer>().material = _meshMaterial;
            _meshFilter = _visionConeVisual.AddComponent<MeshFilter>();

            _visionConeMesh = new Mesh();

            _angleInRadians = _angle;
            _angleInRadians *= Mathf.Deg2Rad;
        }

    
        private void Update()
        {
            DrawVisionCone();
            _enemyStats.isPlayerVisible = CheckVisionCone();
            _isPlayerInDetectionRange = CheckPlayerInDetectionRange();
        }

        private void FixedUpdate()
        {

            if (_enemyController._player == null)
            {
                return;
            }
            if (_isPlayerInDetectionRange)
            {
                RotateTurretTowards(_enemyController._player.transform.position);
            }
            else
            {
                RotateTurretRandom();
            }
        }
        
        private void RotateTurretRandom()
        {
            StartCoroutine(GetRandomPosition());
            Vector3 dir = (_randomDirection - _enemyStats.turret.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            lookRot *= Quaternion.Euler(0, 90, 0);
            _enemyStats.turret.rotation = Quaternion.Lerp(_enemyStats.turret.rotation, lookRot, Time.deltaTime * _enemyStats.turretRotationSpeed);
        }

        private IEnumerator GetRandomPosition()
        {
            float xrandom = Random.Range(-1, 1);
            float zrandom = Random.Range(-1, 1);
            
            float x = xrandom > 0.5f ? 1 : -1;
            float z = zrandom > 0.5f ? 1 : -1;
            
            _randomDirection = new Vector3(x, 0, Random.Range(-1, 1));
            yield return new WaitForSeconds(2);
        }
        
        private void RotateTurretTowards(Vector3 target)
        {
            Vector3 dir = (target - _enemyStats.turret.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            lookRot *= Quaternion.Euler(0, 90, 0);
            _enemyStats.turret.rotation = Quaternion.Lerp(_enemyStats.turret.rotation, lookRot, Time.deltaTime * _enemyStats.turretRotationSpeed);
        }

        private bool CheckPlayerInDetectionRange()
        {
            if (_enemyController._player == null)
            {
                return false;
            }
            
            bool isPlayerInDetectionRange = false;
            
            Vector3 playerPosition = _enemyController._player.transform.position;
            
            if (Vector3.Distance(_enemyStats.firePoint.position, playerPosition) <= _enemyStats.detectionRange)
            {
                isPlayerInDetectionRange = true;
            }
            else
            {
                isPlayerInDetectionRange = false;
            }
            
            return isPlayerInDetectionRange;
        }

        private bool CheckVisionCone()
        {
            if (_enemyController._player == null)
            {
                return false;
            }
            
            bool isPLayerVisible = false;
            
            Vector3 playerPosition = _enemyController._player.transform.position;

            if (Vector3.Distance(_enemyStats.firePoint.position, playerPosition) > _distance)
            {
                return false;
            }
            
            if (Vector3.Angle(_enemyStats.firePoint.forward,  playerPosition - _enemyStats.firePoint.position) > _angle / 2)
            {
                return false;
            }
            
            Vector3 raycastDirection = playerPosition - _enemyStats.firePoint.position;
            if (Physics.Raycast(_enemyStats.firePoint.position, raycastDirection, out RaycastHit hit, _distance))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    isPLayerVisible = true;
                    Debug.Log("Detecto jugador");
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            Debug.DrawRay(_enemyStats.firePoint.position, raycastDirection,Color.black, _distance);
            return isPLayerVisible;

        }
        
        //TODO: Hearing radius and checks

        private void DrawVisionCone() 
        {
            int[] triangles = new int[(_visionConeResolution - 1) * 3];
            Vector3[] Vertices = new Vector3[_visionConeResolution + 1];
            Vertices[0] = Vector3.zero;
            float Currentangle = -_angleInRadians / 2;
            float angleIcrement = _angleInRadians / (_visionConeResolution - 1);
            float Sine;
            float Cosine;

            for (int i = 0; i < _visionConeResolution; i++)
            {
                Sine = Mathf.Sin(Currentangle);
                Cosine = Mathf.Cos(Currentangle);
                Vector3 RaycastDirection = (_enemyStats.firePoint.forward * Cosine) + (_enemyStats.firePoint.right * Sine);
                Vector3 VertForward = (Vector3.forward * Cosine) + (Vector3.right * Sine);
                if (Physics.Raycast(_enemyStats.firePoint.position, RaycastDirection, out RaycastHit hit, _distance,
                        _obstructingVisionLayerMask))
                {
                    Vertices[i + 1] = VertForward * hit.distance;
                }
                else
                {
                    Vertices[i + 1] = VertForward * _distance;
                }


                Currentangle += angleIcrement;
            }

            for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
            {
                triangles[i] = 0;
                triangles[i + 1] = j + 1;
                triangles[i + 2] = j + 2;
            }

            _visionConeMesh.Clear();
            _visionConeMesh.vertices = Vertices;
            _visionConeMesh.triangles = triangles;
            _meshFilter.mesh = _visionConeMesh;
        }
        
        
        
    }
}