using System;
using JetBrains.Annotations;

namespace Player.Models
{
    [Serializable]
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        [CanBeNull] public SaveGame SaveGame { get; set; }
    }
}