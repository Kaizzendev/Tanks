using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public SceneController sceneController;

    public EnemyManager enemyManager;
    public UpgradeManager upgradeManager;
    public LevelManager levelManager;
    public enum GameState
    {
        playing,
        pausing,
        gameOver,
        levelUp,
        reward
    }
    
    public GameState currentState { get; private set; }
    
    public event Action<GameState> onStateChanged;

    private void OnEnable()
    {
        enemyManager.onWaveCleared += HandleWaveCleared;
        upgradeManager.onUpgradeButtonClicked += HandleUpgradeChosen;
        levelManager.onLevelUp += HandleLevelUp;
    }

    private void HandleLevelUp()
    {
        if (currentState != GameState.levelUp)
        {
            return;
        }
        ChangeState(GameState.playing);
    }

    private void HandleUpgradeChosen()
    {
        ChangeState(GameState.levelUp);
    }
    
    private void HandleWaveCleared()
    {
        ChangeState(GameState.reward);
    }

    private void OnDisable()
    {
        enemyManager.onWaveCleared -= HandleWaveCleared;
        upgradeManager.onUpgradeButtonClicked -= HandleUpgradeChosen;
        levelManager.onLevelUp -= HandleLevelUp;
    }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ChangeState(GameState.playing);
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;  
        
        onStateChanged?.Invoke(newState);
        HandleState(newState);
    }

    private void HandleState(GameState state)
    {
        switch (state)
        {
            case GameState.playing:
                Time.timeScale = 1;
                break;
            case GameState.pausing:
                Time.timeScale = 0;
                break;
            case GameState.gameOver:
                Time.timeScale = 0;
                break;
            case GameState.levelUp:
                Time.timeScale = 0;
                break;
            case GameState.reward:
                Time.timeScale = 0;
                break;
            
        }
        Debug.Log($"GameState changed to {state}");
    }
}
