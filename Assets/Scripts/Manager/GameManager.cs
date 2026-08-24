using System;
using Player;
using UnityEngine;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public SceneController sceneController;

        public UpgradeManager upgradeManager;
        public EnemyManager enemyManager;
        public LevelManager levelManager;

        public enum GameState
        {
            Playing,
            Pause,
            GameOver,
            LevelUp,
            Reward
        }

        public GameState CurrentState { get; private set; }

        public event Action<GameState> OnStateChanged;

        private void OnEnable()
        {
            enemyManager.OnWaveCleared += HandleWaveCleared;
            GameEvents.onUpgradeButtonClicked += HandleUpgradeChosen;
            levelManager.OnLevelUp += HandleLevelUp;
        }

        private void HandleLevelUp()
        {
            if (CurrentState != GameState.LevelUp)
            {
                return;
            }

            ChangeState(GameState.Playing);
        }

        private void HandleUpgradeChosen()
        {
            ChangeState(GameState.LevelUp);
        }

        private void HandleWaveCleared()
        {
            ChangeState(GameState.Reward);
        }

        private void OnDisable()
        {
            enemyManager.OnWaveCleared -= HandleWaveCleared;
            GameEvents.onUpgradeButtonClicked -= HandleUpgradeChosen;
            levelManager.OnLevelUp -= HandleLevelUp;
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
            ChangeState(GameState.Playing);
        }

        public void ChangeState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;

            OnStateChanged?.Invoke(newState);
            HandleState(newState);
        }

        private void HandleState(GameState state)
        {
            switch (state)
            {
                case GameState.Playing:
                    Time.timeScale = 1;
                    break;
                case GameState.Pause:
                    Time.timeScale = 0;
                    break;
                case GameState.GameOver:
                    Time.timeScale = 0;
                    break;
                case GameState.LevelUp:
                    Time.timeScale = 0;
                    break;
                case GameState.Reward:
                    Time.timeScale = 0;
                    break;

            }

            Debug.Log($"GameState changed to {state}");
        }
    }
}
