using System;
using System.Collections;
using Networking;
using DTOs.Requests;
using DTOs.Responses;
using UnityEngine;

namespace Services
{
    public class AuthService : MonoBehaviour
    {
        [SerializeField] private ApiClient _apiClient;

        public string Token { get; private set; }
        
        // public IEnumerator GetUsers(Action<List<User>> onSuccess)
        // {
        //     yield return _apiClient.Get<List<User>>("api/auth/users", onSuccess);
        // }

        public IEnumerator Login(string username, string password, Action<LoginResponse> onSuccess)
        {
            LoginRequest request = new()
            {
                Username = username,
                Password = password,
            };
            
            yield return _apiClient.Post<LoginRequest, LoginResponse>("api/auth/login", request, onSuccess);
        }
        
        public IEnumerator Register(string username, string password, Action onSuccess)
        {
            RegisterRequest request = new()
            {
                Username = username,
                Password = password,
            };
            
            yield return _apiClient.Post<RegisterRequest>("api/auth/register", request, onSuccess);
        }


        private void Start()
        {
            StartCoroutine(Register("Caramelo", "Fresa", () =>
            {
               Debug.Log("Usuario registrado");
            }));
            
            StartCoroutine(Login("Caramelo", "Fresa", response =>
            {
                Token = response.Token;
                Debug.Log("Token: " + Token);
            }));

        }
    }
}