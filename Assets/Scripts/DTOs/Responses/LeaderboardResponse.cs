using System;
using JetBrains.Annotations;
using Models;

namespace DTOs.Responses
{
    [Serializable]
    public class LeaderboardResponse
    {
        public string Username { get; set; }
        public int HighestWave { get; set; }
        public int TotalKills {get; set;}
    }
}