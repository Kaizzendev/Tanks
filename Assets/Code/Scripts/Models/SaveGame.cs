using System;

namespace Player.Models
{
    [Serializable]
    public class SaveGame
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public int CurrentWave { get; set; }
        public int HighestWave { get; set; }
        public int TotalKills { get; set; }
    }
}