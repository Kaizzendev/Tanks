using System;

namespace DTOs.Responses
{
    [Serializable]
    public class LoginResponse
    {
        public string Token { get; set; }
    }
}