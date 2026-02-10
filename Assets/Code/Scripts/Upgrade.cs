using UnityEngine;

namespace Player
{
    public enum UpgradeType
    {
        Damage,
        MaxHealth,
        MoveSpeed,
        AttackSpeed,
        CriticChance,
        CriticDamage,
    }

    [CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Upgrade")]
    public class Upgrade : ScriptableObject
    {
        public string upgradeName;
        public string description;

        public UpgradeType upgradeType;
        public float value;

        public Sprite icon;

        
    }
}