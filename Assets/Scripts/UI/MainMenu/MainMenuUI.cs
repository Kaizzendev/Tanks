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
        //TODO: CHECK IF TOKEN IS BEING RECEIVED
        Play();
    }

    public void Play()
    {
        SceneLoader.LoadGame();
    }
    
    public void GoToLoginMenu()
    {
        SceneLoader.LoadLoginMenu();
    }

    public void GoToRegisterMenu()
    {
        SceneLoader.LoadRegisterMenu();
    }

    public void GoToLeaderboardMenu()
    {
        SceneLoader.LoadLeaderboardMenu();
    }

    public void GoToSettingsMenu()
    {
        SceneLoader.LoadSettingsMenu();
    }

    public void GoToAboutMenu()
    {
        SceneLoader.LoadAboutMenu();
    }

    public void Exit()
    {
        Application.Quit();
    }    
}
