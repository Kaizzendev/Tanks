using System;

namespace Player.DTOs
{
    [Serializable]
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}