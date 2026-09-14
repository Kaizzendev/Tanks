using System;
using Manager;
using Map;
using UnityEngine;

namespace Player
{
    public class MapNavigationController: PlayerControllerBase
    {
        
        [SerializeField] internal Transform turretTransform;
        [SerializeField] internal LayerMask _layerMask;


        [SerializeField] private float _mapTravelSpeed = 0.5f;
        
        public event Action<MapNode> OnNodeReached;

        private bool _isMoving;
        
        private MapNode _targetNode;

        
        private void OnEnable()
        {
            MapManager.Instance.OnNodeSelected += NodeReceived;
        }
        
        private void OnDisable()
        {
            MapManager.Instance.OnNodeSelected -= NodeReceived;
        }


        private void FixedUpdate()
        {
            RotateTurretTowardsMouse();

            if (_isMoving)
            {
                MoveToNode();
            }
            
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


        private void NodeReceived(MapNode node)
        {
            
            _targetNode = node;
            _isMoving = true;
        }

        private void MoveToNode()
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetNode.Position, _mapTravelSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _targetNode.Position) < 0.9f)
            {
                _isMoving = false;
                OnNodeReached?.Invoke(_targetNode);
            }
        }
        
        
    }
}