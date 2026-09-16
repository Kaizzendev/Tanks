using System;
using UnityEngine;

namespace Networking
{
    public class SessionManager: MonoBehaviour
    {
        
        public static SessionManager Instance;
        
        public string Token { get; private set; }
        
        public bool IsLoggedIn { get; private set; }
        
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

        public void SetToken(string token)
        {
            Token = token;
            IsLoggedIn =  true;
        }
        
    }
}