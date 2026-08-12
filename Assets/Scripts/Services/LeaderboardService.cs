using System;
using System.Collections;
using System.Collections.Generic;
using DTOs.Responses;
using Networking;
using UnityEngine;

namespace Services
{
    public class LeaderboardService: MonoBehaviour 
    {
        [SerializeField] private ApiClient _apiClient;

        public IEnumerator GetLeaderboard(Action<List<LeaderboardResponse>> onSuccess, Action<Exception> onError)
        {
            yield return _apiClient.Get<List<LeaderboardResponse>>("api/leaderboard",onSuccess, onError );
        }
    }
}