using System.Collections.Generic;
using UnityEngine;

namespace ProceduralGeneration
{
    [CreateAssetMenu(fileName = "Encounter", menuName = "Scriptable Objects/Encounters")]
    public class Encounter: ScriptableObject
    {
        [Header("Encounter type")]
        public EncounterType encounterType;
        
        [Header("Room type")]
        public Room roomType;
        
        [Header("Biome")]
        public Biome biome;
        
        [Header("Environment size")]
        public float environmentSize;
         
        [Header("Enemies")]
        public GameObject[] enemies;
        public int enemyCount = 10;
        public float enemyMinDistance = 8f;
        public float enemyHealthMultiplier = 1f; 
        public float enemyDamageMultiplier = 1f;
        
        [Header("Patrol Points")]
        public GameObject patrolPointprefab;
        public float patrolPointsMinDistance = 5f;
        public int  patrolPointsCount = 10;
         
    }
}