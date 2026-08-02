using System;

namespace Player.DTOs
{
    [Serializable]
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}