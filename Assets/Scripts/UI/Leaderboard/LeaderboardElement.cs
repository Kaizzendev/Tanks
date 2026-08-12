using TMPro;
using UnityEngine;

namespace UI.Leaderboard
{
    public class LeaderboardElement: MonoBehaviour
    {
        [SerializeField] internal TMP_Text _leaderboardRank;
        [SerializeField] internal TMP_Text _leaderboardName;
        [SerializeField] internal TMP_Text _leaderboardScore;
    }
}