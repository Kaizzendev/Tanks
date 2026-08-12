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

        public IEnumerator Login(string username, string password, Action<LoginResponse> onSuccess, Action<Exception> onError)
        {
            LoginRequest request = new()
            {
                Username = username,
                Password = password,
            };
            
            yield return _apiClient.Post<LoginRequest, LoginResponse>("api/auth/login", request, onSuccess, onError);
        }
        
        public IEnumerator Register(string username, string password, Action onSuccess, Action<Exception> onError)
        {
            RegisterRequest request = new()
            {
                Username = username,
                Password = password,
            };
            
            yield return _apiClient.Post<RegisterRequest>("api/auth/register", request, onSuccess, onError);
        }
        
    }
}