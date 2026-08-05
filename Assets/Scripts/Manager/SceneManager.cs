using UnityEngine;
using UnityEngine.SceneManagement;
namespace Manager
{
    public static class SceneManager
    {
        public static void LoadScene(string sceneName)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }

        public static void ReloadScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        }

        public static void Quit()
        {
            Application.Quit();
        }
        
        public static void LoadMainMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }

        public static void LoadGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }

        public static void LoadLoginMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
        }

        public static void LoadRegisterMenu()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Register");
        }
    }
}