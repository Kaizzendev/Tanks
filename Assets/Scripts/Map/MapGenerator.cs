using System.Collections.Generic;
using ProceduralGeneration;
using UnityEngine;

namespace Map
{
    public class MapGenerator
    {
        private List<MapNode> mapNodes = new List<MapNode>();

        [Header("Map Configuration")] 
        [SerializeField] private int _numberOfLayers = 5;

        [SerializeField] private int _maxChildPerNode = 3;
        

        private void Generate()
        {
            mapNodes.Clear();
            
            GenerateTree();
        }
        private void GenerateTree()
        {
            int currentLayer = 1;
            int nodeId = 0;
            while (currentLayer <= _numberOfLayers)
            {
                if (currentLayer == 1)
                {
                    GenerateNode(currentLayer,nodeId);
                    currentLayer++;
                }

                for (int i = 0; i < 5; i++)
                {
                    GenerateNode(currentLayer, nodeId);
                }

                currentLayer++;
            }
        }

        private void GenerateNode(int layer,  int nodeId)
        {
            MapNode node = new MapNode();
            node.Layer = layer;
            node.EncounterType = (EncounterType) Random.Range(0, (int) EncounterType.Upgrade);
        }
    }
}