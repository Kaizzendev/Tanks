using System;
using Manager;
using UnityEngine;

namespace Map
{
    public class MapNodeView: MonoBehaviour
    {
        
        private MapNode _mapNode;

        private Color _color;
        
        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        internal void Initialize(MapNode mapNode, Color color)
        {
            _mapNode = mapNode;
            _color = color;
            
            _meshRenderer.material.color = color;
        }

        private void OnMouseDown()
        {
            MapManager.Instance.SelectNode(_mapNode);
        }


    }
}