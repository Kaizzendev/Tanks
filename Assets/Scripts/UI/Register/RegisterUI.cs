using System;
using DTOs.Responses;
using Manager;
using Services;
using TMPro;
using UnityEngine;

public class RegisterUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private TextMeshProUGUI usernameErrorText;
    [SerializeField] private AuthService _authService;

    private void Start()
    {
        usernameErrorText.gameObject.SetActive(false);
    }

    public void OnRegisterPressed()
    {
        if (usernameInput.text != "" && passwordInput.text != "")
        {
            StartCoroutine(_authService.Register(usernameInput.text, passwordInput.text, OnRegisterSuccess, OnRegisterError));
        }
        else
        {
            OnRegisterError("Empty field");
        }   
    }

    private void OnRegisterSuccess()
    {
        StartCoroutine(_authService.Login(usernameInput.text,passwordInput.text, OnLoginSuccess, OnLoginError));
    }

    private void OnLoginSuccess(LoginResponse response)
    {
        SceneManager.LoadMainMenu();
    }
    
    private void OnRegisterError(Exception ex)
    {
        usernameErrorText.gameObject.SetActive(true);
        usernameErrorText.text = "Username already exists.";
        Debug.LogError(ex);
    }
    
    private void OnRegisterError(string errorMessage)
    {
        usernameErrorText.gameObject.SetActive(true);
        usernameErrorText.text = errorMessage;
    }

    private void OnLoginError(Exception ex)
    {
        Debug.LogError(ex);
    }

    public void GoToLoginMenu()
    {
        SceneManager.LoadLoginMenu();
    }
    
    public void GoBack()
    {
        SceneManager.LoadMainMenu();
    }
    
}
