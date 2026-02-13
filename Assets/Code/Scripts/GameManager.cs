using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public SceneController sceneController;

    public EnemyManager enemyManager;
    public UpgradeManager upgradeManager;
    public enum GameState
    {
        playing,
        pausing,
        gameOver,
        victory,
        reward
    }
    
    public GameState currentState { get; private set; }
    
    public event Action<GameState> onStateChanged;

    private void OnEnable()
    {
        enemyManager.onWaveCleared += HandleWaveCleared;
        upgradeManager.onUpgradeButtonClicked += HandleUpgradeChosen;
    }

    private void HandleUpgradeChosen()
    {
        ChangeState(GameState.playing);
    }

    private void OnDisable()
    {
        enemyManager.onWaveCleared -= HandleWaveCleared;
        upgradeManager.onUpgradeButtonClicked -= HandleUpgradeChosen;
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
            case GameState.victory:
                Time.timeScale = 0;
                break;
            case GameState.reward:
                Time.timeScale = 0;
                break;
            
        }
        Debug.Log($"GameState changed to {state}");
    }


    
    private void HandleWaveCleared()
    {
        ChangeState(GameState.reward);
    }
    
}
