using System;

namespace Player.DTOs
{
    [Serializable]
    public class LoginResponse
    {
        public string Token { get; set; }
    }
}