using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] ProceduralMap proceduralMap;
    public int currentLevel = 0;
    
    public LevelConfig config;

    public event Action onLevelUp;
    
#if UNITY_EDITOR
    [ContextMenu("Generate Map")]
#endif
    public void GenerateContextMenu()
    {
        StartLevel(config);
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
        Debug.Log("Starting Level");
        StartLevel(config);
    }

    private void SetDifficultyLevel(LevelConfig config, int currentLevel)
    {
        config.seed += currentLevel * 13;
        config.mapSize = new Vector2(Random.Range(10f,300f), Random.Range(10f,300f));
        config.enemyCount += Mathf.RoundToInt(currentLevel * 1.5f);
        Debug.Log("Difficulty Level: " + currentLevel);
    }
    
    private void StartLevel(LevelConfig config) // Configure player spawn and delete current active missiles
    {
        config.seed = Random.Range(-1000, 1000);
        proceduralMap.Generate(config.seed, config.biome, config.mapSize, 
            config.objectSpacing, config.noiseScale, config.enemyMinDistance, config.enemyCount);
        onLevelUp?.Invoke();
    }


    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.levelUp)
        {
            GoToNextLevel();
        }

        if (state == GameManager.GameState.playing)
        {
            StartLevel(config);
        }
    }

    private void GoToNextLevel()
    {
        currentLevel++;
        SetDifficultyLevel(config, currentLevel);
        StartLevel(config);
    }
}
