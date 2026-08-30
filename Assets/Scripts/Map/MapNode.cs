using System.Collections.Generic;
using ProceduralGeneration;

namespace Map
{
    public class MapNode
    {
        internal float Id;
        internal EncounterType EncounterType;
        internal int Layer;
        internal int seed;
        
        internal List<MapNode> Neighbours;

        public override string ToString()
        {
            return $"Id: {Id}, EncounterType: {EncounterType}, Layer: {Layer}, seed: {seed} ";
        }
    }
}