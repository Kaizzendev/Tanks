using System;
using Manager;
using Services;
using TMPro;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject accountPanel;

    [SerializeField] private AuthService _authService;
    private void Start()
    {
        accountPanel.SetActive(false);
    }

    public void OpenAccountPopUpMenu()
    {
        if (_authService.Token != null)
        {
            Play();
        }
        else
        {
            accountPanel.SetActive(true);
        }
    }

    public void Play()
    {
        SceneManager.LoadGame();
    }
    
    public void GoToLoginMenu()
    {
        SceneManager.LoadLoginMenu();
    }

    public void GoToRegisterMenu()
    {
        SceneManager.LoadRegisterMenu();
    }

    public void GoToLeaderboardMenu()
    {
        SceneManager.LoadLeaderboardMenu();
    }

    public void GoToSettingsMenu()
    {
        SceneManager.LoadSettingsMenu();
    }

    public void GoToAboutMenu()
    {
        SceneManager.LoadAboutMenu();
    }

    public void Exit()
    {
        Application.Quit();
    }    
}
