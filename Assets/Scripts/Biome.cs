using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Biome", menuName = "Scriptable Objects/Biome")]
public class Biome : ScriptableObject
{
    [Header("Terrain")] 
    public Material terrainMaterial;
    
    [Header("Props")]
    public List<ProceduralObject> proceduralObjects;
    
    [Header("Settings")]
    public float objectNoiseScale = 0.05f;
    public float baseObjectDensity = 0.3f;
}
