using System;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Manager
{
    public static class SceneLoader
    {
        public static event Action<Scene,LoadSceneMode> sceneLoaded;
        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public static void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public static void Quit()
        {
            Application.Quit();
        }
        
        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public static void LoadGame()
        {
            SceneManager.LoadScene("Map");
        }

        public static void LoadLoginMenu()
        {
            SceneManager.LoadScene("Login");
        }

        public static void LoadRegisterMenu()
        {
            SceneManager.LoadScene("Register");
        }
        
        public static void LoadLeaderboardMenu()
        {
            SceneManager.LoadScene("Leaderboard");
        }

        public static void LoadSettingsMenu()
        {
            SceneManager.LoadScene("Settings");
        }

        public static void LoadAboutMenu()
        {
            SceneManager.LoadScene("About");
        }

        public static void LoadLevel()
        {
            SceneManager.LoadScene("Level");
        }
    }
}