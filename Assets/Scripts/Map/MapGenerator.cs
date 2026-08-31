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

        [Header("Debug")]
        [SerializeField] private bool _isDebugMode = false;
        
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

        private EncounterTypeGenerationRule[] _encounterTypeGenerationRules;
        
        
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
            SetNodesEncounterType();
            SetNodeConnections();
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
            
            for (int i = currentLayer; i <= _numberOfLayers; i++)
            {
                int numberOfNodesOnLayer = GetDensityPerLayer(currentLayer);
                List<MapNode> currentNodesInLayer = new List<MapNode>(numberOfNodesOnLayer);

                if (currentLayer == _numberOfLayers)
                {
                    numberOfNodesOnLayer = 1;
                }
                
                for (int j = 0; j < numberOfNodesOnLayer; j++)
                {
                    nodeId = int.Parse(currentLayer + j.ToString());
                    MapNode currentNode = GenerateNode(currentLayer, nodeId);
                    currentNodesInLayer.Add(currentNode);
                    _mapNodes.Add(currentNode);
                }
                _nodesPerLayer.Add(currentNodesInLayer);
                currentLayer++;
            }
        }

        private void SetNodesEncounterType() 
        {
            
            _encounterTypeGenerationRules = Resources.LoadAll<EncounterTypeGenerationRule>("ScriptableObjects/Map Rules");
            
            for (int i = 0; i < _nodesPerLayer.Count; i++)
            {
                for (int j = 0; j < _nodesPerLayer[i].Count; j++)
                {
                    _nodesPerLayer[i][j].EncounterType = SetEncounterTypePerLayer(i);
                    DebugLog($"In layer {i}, node {j} --> {_nodesPerLayer[i][j].ToString()}");
                }
            }
        }

        private EncounterType SetEncounterTypePerLayer(int currentLayer)
        {
            EncounterType encounterType = EncounterType.Combat;

            if (currentLayer == 0)
            {
                return EncounterType.Start;
            }
            
            if (currentLayer == _numberOfLayers)
            {
                return EncounterType.Boss;
            }
            
            List<EncounterTypeGenerationRule> encounterTypeCandidates = new List<EncounterTypeGenerationRule>();
            
            float mapProgress = (float)currentLayer / _numberOfLayers;
            
            foreach (EncounterTypeGenerationRule encounterTypeGenerationRule in _encounterTypeGenerationRules)
            {
                if (mapProgress >= encounterTypeGenerationRule.MinMapProgress &&
                    mapProgress <= encounterTypeGenerationRule.MaxMapProgress)
                {
                    encounterTypeCandidates.Add(encounterTypeGenerationRule);
                }
            }
            
            List<float> encounterTypeProbabilities = new List<float>();

            foreach (EncounterTypeGenerationRule candidate in encounterTypeCandidates)
            {
                encounterTypeProbabilities.Add(candidate.AnimationCurve.Evaluate(((float)currentLayer / (float)_numberOfLayers)));
            }

            float maxWeight = 0;
            for (int i = 0; i < encounterTypeProbabilities.Count; i++)
            {
                maxWeight += encounterTypeProbabilities[i];
            }
            float accumulatedWeight = 0;
            float weight = Random.Range(0, maxWeight);

            for (int i = 0; i < encounterTypeProbabilities.Count; i++)
            {
                accumulatedWeight += encounterTypeProbabilities[i];

                if (weight <= accumulatedWeight)
                {
                    encounterType = encounterTypeCandidates[i].EncounterType;
                    break;
                }
            }
            


            return encounterType;
        }
        
        private void SetNodeConnections()
        {
            for (int i = 0; i < _nodesPerLayer.Count -1; i++)
            {
                for (int j = 0; j < _nodesPerLayer[i].Count; j++)
                {
                    int desiredChildNodes = Random.Range(_minChildPerNode, Mathf.Min(_maxChildPerNode, _nodesPerLayer[i + 1].Count + 1));

                    foreach (MapNode currentNode in _nodesPerLayer[i + 1])
                    {
                        if (currentNode.EncounterType == EncounterType.Boss)
                        {
                            _nodesPerLayer[i][j].Children.Add(currentNode);
                            currentNode.Parents.Add(_nodesPerLayer[i][j]);
                            DebugLog($"Adding child node {currentNode.ToString()} --> {_nodesPerLayer[i][j].ToString()}");
                            continue;
                        }
                        if (_nodesPerLayer[i][j].Children.Count >= desiredChildNodes)
                        {
                            break;
                        }
                        
                        if (currentNode.Parents.Count >= 1) //random min parents = 1, max parents = nodos totales de la capa anterior
                        {
                            continue;
                        }

                        if (_nodesPerLayer[i][j].Children.Contains(currentNode))
                        {
                            continue;
                        }

                        if (currentNode.Parents.Contains(_nodesPerLayer[i][j]))
                        {
                            continue;
                        }
                        
                        _nodesPerLayer[i][j].Children.Add(currentNode);
                        currentNode.Parents.Add(_nodesPerLayer[i][j]);
                        DebugLog($"Adding child node {currentNode.ToString()} --> {_nodesPerLayer[i][j].ToString()}");
                    }
                    
                }
            }

            for (int i = 0; i < _nodesPerLayer.Count -1; i++)
            {
                foreach (MapNode childrenNode in _nodesPerLayer[i +1])
                {
                    if (childrenNode.Parents.Count == 0)
                    {
                        foreach (MapNode currentNode in _nodesPerLayer[i])
                        {
                            if (childrenNode.Parents.Contains(currentNode))
                            {
                                continue;
                            }

                            if (currentNode.Children.Count == _maxChildPerNode)
                            {
                                continue;
                            }
                            
                            childrenNode.Parents.Add(currentNode);
                            currentNode.Children.Add(childrenNode);
                            DebugLog($"Adding child node {childrenNode.ToString()} --> {currentNode.ToString()}");
                            break;
                        }
                    }
                }
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
            node.Id = nodeId;
            node.seed = seed;
            node.seed += nodeId;
            node.Layer = layer;
            return node;
        }

        private void DebugLog(string message)
        {
            if (_isDebugMode)
            {
                Debug.Log($"[MAP GENERATOR] {message}");
            }    
        }
        
        
    }
}