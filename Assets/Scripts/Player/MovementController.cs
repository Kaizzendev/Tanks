using System;
using UnityEngine;

namespace Player
{
    public class MovementController: PlayerControllerBase
    {
        private Rigidbody _rb;
        private float _moveInput;
        private float _rotationInput;
        
        [SerializeField] internal Transform turretTransform;
        [SerializeField] internal LayerMask _layerMask;


        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
        }

        internal void Update()
        {
            if (!isEnabled)
            {
                return;
            }
            
            _moveInput = Input.GetAxis("Vertical");
            _rotationInput = Input.GetAxis("Horizontal");
        }

        private void FixedUpdate()
        {
            MoveTank(_moveInput);
            RotateTank(_rotationInput);
            RotateTurretTowardsMouse();
        }
        
        private void RotateTank(float input)
        {
            Quaternion rotation = Quaternion.Euler(new Vector3(0, PlayerStats.Instance.rotationSpeed * input, 0));
            _rb.MoveRotation(_rb.rotation * rotation);
        }

        private void MoveTank(float input)
        {
            Vector3 move = input * transform.forward * PlayerStats.Instance.moveSpeed;
            _rb.linearVelocity = new Vector3(
                Mathf.Clamp(move.x, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed),
                _rb.linearVelocity.y,
                Mathf.Clamp(move.z, -PlayerStats.Instance.maxMoveSpeed, PlayerStats.Instance.maxMoveSpeed)
            );
        }
    
        private void RotateTurretTowardsMouse()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Vector3 mousePos = Vector3.zero;
            if (Physics.Raycast(ray, out RaycastHit hit, _layerMask))
            {
                mousePos = hit.point;
            }

            Vector3 direction = new Vector3(mousePos.x, 0, mousePos.z) -
                                new Vector3(turretTransform.position.x, 0, turretTransform.position.z);

            turretTransform.rotation = Quaternion.LookRotation(direction);
        }
        
    }
}