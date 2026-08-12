using System;
using UnityEngine;

namespace Player
{
    public static class GameEvents
    {
        public static Action onUpgradeButtonClicked;
        public static Action<Transform> onPlayerSpawn;
    }
}