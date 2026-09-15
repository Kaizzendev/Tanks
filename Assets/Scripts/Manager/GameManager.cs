using System;
using Player;
using ProceduralGeneration;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public enum GameState
        {
            Playing,
            Pause,
            GameOver,
            Reward,
            LoadingLevel,
            MapNavigation
        }

        [HideInInspector] public Encounter CurrentEncounterData;

        public GameState CurrentState { get; private set; }

        public event Action<GameState> OnStateChanged;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            GameEvents.OnLevelGenerated += LevelReady;
            EnemyEvents.OnWaveCleared += HandleWaveCleared;
            GameEvents.onUpgradeButtonClicked += ReturnToMap;
        }
        
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private void ReturnToMap()
        {
            SceneLoader.LoadGame();
        }

        private void LevelReady()
        {
            ChangeState(GameState.Playing);
        }
        
        private void HandleWaveCleared()
        {
            ChangeState(GameState.Reward);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (scene.name)
            {
                case "Map":
                    ChangeState(GameState.MapNavigation);
                    break;
                case "Level":
                    ChangeState(GameState.LoadingLevel);
                    break;
            }
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
                case GameState.Reward:
                    Time.timeScale = 1;
                    break;
                case GameState.MapNavigation:
                    Time.timeScale = 1;
                    break;
            }

            Debug.Log($"GameState changed to {state}");
        }
    }
}
