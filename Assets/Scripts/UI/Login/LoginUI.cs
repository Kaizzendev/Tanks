using System;
using DTOs.Responses;
using Manager;
using Services;
using TMPro;
using UnityEngine;

public class LoginUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI loginErrorText;
    [SerializeField] private AuthService _authService;

    private void Start()
    {
        loginErrorText.gameObject.SetActive(false);
    }

    public void OnLoginPressed()
    {
        if (usernameInput.text != "" && passwordInput.text != "")
        {
            StartCoroutine(_authService.Login(usernameInput.text, passwordInput.text, OnLoginSuccess, OnLoginError));
        }
        else
        {
            OnLoginError("Empty field");
        }   
        
    }

    private void OnLoginSuccess(LoginResponse response)
    {
        SceneManager.LoadMainMenu();
    }

    private void OnLoginError(Exception ex)
    {
        loginErrorText.gameObject.SetActive(true);
        loginErrorText.text = "Username or password is incorrect";
        Debug.LogError(ex);
    }

    private void OnLoginError(string errorMessage)
    {
        loginErrorText.gameObject.SetActive(true);
        loginErrorText.text = errorMessage;
    }

    public void GoToRegisterMenu()
    {
        SceneManager.LoadRegisterMenu();
    }
    
}
