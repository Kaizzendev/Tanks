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
    public GameObject[] enemies;
    public int enemyCount = 10;
    public float enemyMinDistance = 8f;
    public float enemyHealthMultiplier = 1f; 
    public float enemyDamageMultiplier = 1f;
    
    [Header("Patrol Points")]
    public GameObject patrolPointprefab;
    public float patrolPointsMinDistance = 5f;
    public int  patrolPointsCount = 10;

    [Header("Difficulty")]
    public int difficultyLevel = 1;

    [Header("Optional Rewards / Powerups")]
    public int maxPowerUps = 2;

    public bool generatePlayer = true;

    public override string ToString()
    {
        return $"LevelConfig:\n" +
               $"- Map Size: {mapSize.x} x {mapSize.y}\n" +
               $"- Object Spacing: {objectSpacing}\n" +
               $"- Seed: {seed}\n" +
               $"- Noise Scale: {noiseScale}\n" +
               $"- Biome: {(biome != null ? biome.name : "None")}\n" +
               $"- Enemy Count: {enemyCount}\n" +
               $"- Enemy Min Distance: {enemyMinDistance}\n" +
               $"- Enemy Health Multiplier: {enemyHealthMultiplier}\n" +
               $"- Enemy Damage Multiplier: {enemyDamageMultiplier}\n" +
               $"- Patrol Points Count: {patrolPointsCount}\n" +
               $"- Patrol Points Min Distance: {patrolPointsMinDistance}\n" +
               $"- Generate Player: {generatePlayer}\n" +
               $"- Difficulty Level: {difficultyLevel}\n" +
               $"- Max PowerUps: {maxPowerUps}";
    }
}
