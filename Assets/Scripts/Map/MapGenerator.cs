using System;
using System.Collections.Generic;
using System.Linq;
using ProceduralGeneration;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map
{
    public class MapGenerator: MonoBehaviour
    {
        private List<MapNode> _mapNodes = new List<MapNode>();

        [Header("Map Configuration")] 
        [SerializeField] private int _numberOfLayers = 5;

        [SerializeField] private int _maxChildPerNode = 3;
        
        [SerializeField] private int _minChildPerNode = 1;
        
        [SerializeField] private int _maxNodesPerLayer = 6;
        
        [SerializeField] private int _minNodesPerLayer = 1;

        [SerializeField] private AnimationCurve _animationCurve;
        
        private List<int> _densityPerLayer = new List<int>();
        
        private List<List<MapNode>> _nodesPerLayer = new List<List<MapNode>>();

        public int seed;
        
        
#if UNITY_EDITOR
        [ContextMenu("Generate Map")]
#endif
        public void GenerateContextMenu()
        {
            Generate();
        }
        
        private void Generate()
        { 
            Clear();
            GenerateSeed();
            GenerateGraph();
        }
        
        private void GenerateSeed()
        {
            Random.InitState(seed);
        }

        private void Clear()
        {
            _mapNodes.Clear();
            _densityPerLayer.Clear();
            _nodesPerLayer.Clear();
        }
        
        private void GenerateGraph()
        {
            int currentLayer = 0;
            int nodeId = 0;

            MapNode initialNode = GenerateNode(currentLayer, nodeId);
            List<MapNode> initialNodeList = new List<MapNode>();
            initialNodeList.Add(initialNode);
            
            
            _mapNodes.Add(initialNode);
            _nodesPerLayer.Capacity = _numberOfLayers;
            _nodesPerLayer.Add(initialNodeList);
            currentLayer++;
            
            while (currentLayer <= _numberOfLayers)
            {
                int numberOfNodesOnLayer = GetDensityPerLayer(currentLayer);
                List<MapNode> currentNodesInLayer = new List<MapNode>(numberOfNodesOnLayer);
                for (int i = 0; i < numberOfNodesOnLayer; i++)
                {
                    nodeId = int.Parse(currentLayer.ToString() + i.ToString());
                    MapNode currentNode = GenerateNode(currentLayer, nodeId);
                    currentNodesInLayer.Add(currentNode);
                    _mapNodes.Add(currentNode);
                }
                _nodesPerLayer.Add(currentNodesInLayer);
                Debug.Log($"Generated {_nodesPerLayer[currentLayer].Count}, in layer {currentLayer}");
                currentLayer++;
            }
        }

        private int GetDensityPerLayer(int layer)
        {
            float density = _animationCurve.Evaluate(((float)layer/(float)_numberOfLayers));
            
            int roundedDensity = (int)Mathf.Round(density * _maxNodesPerLayer);
            if (roundedDensity < _minNodesPerLayer)
            {
                roundedDensity = _minNodesPerLayer;
            }
            return roundedDensity;
        }

        private MapNode GenerateNode(int layer,  int nodeId, EncounterType encounterType = EncounterType.Combat)
        {
            MapNode node = new MapNode();
            node.seed = seed + nodeId;
            node.Layer = layer;
            return node;
        }
    }
}