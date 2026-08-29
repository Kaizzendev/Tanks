using System.Collections.Generic;
using ProceduralGeneration;

namespace Map
{
    public class MapNode
    {
        internal int Id;
        internal int Layer;
        internal EncounterType EncounterType;
        internal int seed;
        
        internal List<MapNode> Neighbours; 
    }
}