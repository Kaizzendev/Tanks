using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Scriptable Objects/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [Header("Procedural Map")]
    public Vector2 mapSize = new Vector2(10,10);
    public float objectSpacing = 5f;
    public int seed = 12345;
    public float noiseScale = 0.05f;

    [Header("Biome")]
    public Biome biome;

    [Header("Enemies")]
    public int enemyCount = 10;
    public float enemyMinDistance = 8f;
    public float enemyHealthMultiplier = 1f; 
    public float enemyDamageMultiplier = 1f;
    
    [Header("Patrol Points")]
    public float patrolPointsMinDistance = 5f;
    public int  patrolPointsCount = 10;

    [Header("Difficulty")]
    public int difficultyLevel = 1;

    [Header("Optional Rewards / Powerups")]
    public int maxPowerUps = 2;
    
}
