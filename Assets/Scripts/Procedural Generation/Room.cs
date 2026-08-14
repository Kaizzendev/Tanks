using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration
{
    [CreateAssetMenu(fileName = "Room", menuName = "Scriptable Objects/Rooms")]
    public class Room: ScriptableObject
    {
        [Header("Size")] 
        public Vector2 mapSize;
    
        [Header("Props")]
        public List<ProceduralObject> proceduralObjects;

        [Header("Walls")]
        public GameObject wall;
        public bool physicalWall = false;
    
        [Header("Settings")]
        public float objectNoiseScale = 0.05f;
        public float baseObjectDensity = 0.3f;
        public float objectSpacing = 5f;
    }
}