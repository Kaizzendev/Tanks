using System.Collections.Generic;
using ProceduralGeneration;
using UnityEngine;

namespace Map
{
    public class MapNode
    {
        internal float Id;
        internal EncounterType EncounterType;
        internal int Layer;
        internal int seed;

        internal Vector3 Position;
        
        internal List<MapNode> Parents = new List<MapNode>();
        
        internal List<MapNode> Children = new List<MapNode>();

        public override string ToString()
        {
            return $"Id: {Id}, EncounterType: {EncounterType}, Layer: {Layer}, seed: {seed} ";
        }

        public string GetChildrenMessage()
        {
            string result = "";
            foreach (var node in Children)
            {
                result += node.ToString();
            }
            return result;
        }
        
        public string GetParentsMessage()
        {
            string result = "";
            foreach (var node in Parents)
            {
                result += node.ToString();
            }
            return result;
        }
    }
}