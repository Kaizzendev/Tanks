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

        public void Apply()
        {
            var stats = PlayerStats.Instance;

            switch (upgradeType)
            {
                case UpgradeType.Damage:
                    stats.damage += value;
                    break;
                case UpgradeType.MaxHealth:
                    stats.maxHealth += value;
                    break;
                case UpgradeType.MoveSpeed:
                    stats.moveSpeed += value;
                    break;
                case UpgradeType.AttackSpeed:
                    stats.attackSpeed += value; //TODO: Hmm
                    break;
                case UpgradeType.CriticChance:
                    stats.criticChance += value;
                    break;
                case UpgradeType.CriticDamage:
                    stats.criticMultiplier += value;
                    break;
            }
        }
    }
}