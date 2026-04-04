using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] ProceduralMap proceduralMap;
    public int currentLevel = 1;
    
    public LevelConfig defaultConfig;

    public event Action onLevelUp;
    
#if UNITY_EDITOR
    [ContextMenu("Generate Map")]
#endif
    public void GenerateContextMenu()
    {
        StartLevel(defaultConfig);
    }
    
    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        GameManager.Instance.onStateChanged += OnGameStateChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.onStateChanged -= OnGameStateChanged;
    }

    private void Start()
    {
        LevelConfig initialConfig = SetDifficultyLevel(defaultConfig,currentLevel);
        StartLevel(initialConfig);
    }

    private LevelConfig SetDifficultyLevel(LevelConfig config, int currentLevel)
    {
        LevelConfig newConfig = Instantiate(config);
        newConfig.seed = config.seed + currentLevel * 13;
        newConfig.mapSize = new Vector2(Random.Range(10f,300f), Random.Range(10f,300f));
        newConfig.enemyCount = config.enemyCount + Mathf.RoundToInt(currentLevel * 1.2f);
        
        int length = 1;
        if (currentLevel == 1) config.seed = Random.Range(-1000, 1000);
        if (currentLevel >= 3) length = 2;
        if (currentLevel >= 7) length = config.enemies.Length;

        newConfig.enemies = new GameObject[length];
        Array.Copy(config.enemies, 0, newConfig.enemies, 0, length);

        
        Debug.Log("Difficulty Level: " + currentLevel);
        return newConfig; 
    }
    
    private void StartLevel(LevelConfig config) 
    {
        proceduralMap.Generate(
            config.seed, 
            config.biome, 
            config.mapSize, 
            config.objectSpacing, 
            config.noiseScale, 
            config.enemies,
            config.enemyMinDistance, 
            config.enemyCount,
            config.patrolPointprefab,
            config.patrolPointsMinDistance, 
            config.patrolPointsCount,
            config.generatePlayer
            );
    }


    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.levelUp)
        {
            GoToNextLevel();
        }
    }

    private void GoToNextLevel()
    {
        StartCoroutine(GoToNextLevelCoroutine());
        
    }

    private IEnumerator GoToNextLevelCoroutine()
    {
        currentLevel++;
        LevelConfig newLevel = SetDifficultyLevel(defaultConfig, currentLevel);
        StartLevel(newLevel);
        yield return new WaitForEndOfFrame();
        onLevelUp?.Invoke();
    }
}
