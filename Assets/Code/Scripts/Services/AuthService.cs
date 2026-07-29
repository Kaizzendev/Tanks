using System;
using System.Collections;
using System.Collections.Generic;
using Networking;
using Player.Models;
using UnityEngine;

namespace Player.Services
{
    public class AuthService : MonoBehaviour
    {
        [SerializeField] private ApiClient _apiClient;

        public IEnumerator GetUsers(Action<List<User>> onSuccess)
        {
            yield return _apiClient.Get<List<User>>("api/auth/users", onSuccess);
        }


        private void Start()
        {
            StartCoroutine(GetUsers(users =>
            {
                foreach (User user in users)
                {
                    Debug.Log("ID: " + user.id + " Username: " + user.username + " PasswordHash: " + user.passwordHash);
                }
            }));

        }
    }
}