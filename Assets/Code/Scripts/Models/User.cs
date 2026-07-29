using System;

namespace Player.Models
{
    [Serializable]
    public class User
    {
        public string id;
        public string username;
        public string passwordHash;
    }
}