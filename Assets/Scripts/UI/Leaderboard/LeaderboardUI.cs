using System;
using System.Collections.Generic;
using DTOs.Responses;
using Manager;
using UI.Leaderboard;
using Services;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Leaderboard
{
    public class LeaderboardUI : MonoBehaviour
    {
        [SerializeField] private LeaderboardService _leaderboardService;
        [SerializeField] private GameObject _leaderboardContent;
        [SerializeField] private LeaderboardElement _leaderboardItemPrefab;
        private List<LeaderboardElement> _leaderboardItems = new List<LeaderboardElement>();

        private void Start()
        {

            StartCoroutine(_leaderboardService.GetLeaderboard(OnLeaderboardSuccess, OnError));
        }

        private void OnLeaderboardSuccess(List<LeaderboardResponse> leaderboard)
        {
            LoadLeaderboard(leaderboard);
        }

        private void OnError(Exception ex)
        {
            LeaderboardElement leaderboardItem = Instantiate(_leaderboardItemPrefab, _leaderboardContent.transform);
            leaderboardItem._leaderboardName.text = ex.Message;
        }
        
        private void LoadLeaderboard(List<LeaderboardResponse> leaderboard)
        {
            _leaderboardItems.Clear();
            for (int i = 0; i < leaderboard.Count; i++)
            {
                LeaderboardElement leaderboardItem = Instantiate(_leaderboardItemPrefab, _leaderboardContent.transform);
                leaderboardItem._leaderboardName.text = leaderboard[i].Username;
                leaderboardItem._leaderboardScore.text = leaderboard[i].HighestWave.ToString();
                leaderboardItem._leaderboardRank.text = (i +1).ToString();
                _leaderboardItems.Add(leaderboardItem);
            }

            foreach (var leaderboardElement in leaderboard)
            {
                Debug.Log("user: " + leaderboardElement.Username + "/ ronda: " + leaderboardElement.HighestWave);
            }
        }

        public void GoBack()
        {
            SceneManager.LoadMainMenu();
        }

    }
}
