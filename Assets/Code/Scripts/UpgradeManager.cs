using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    
    private UpgradeButtonUI upgradeButtonUI;
    [Header("UI References")] public UpgradeButtonUI[] upgradeButtons;

    private Upgrade[] allUpgrades;
    private Upgrade[] upgradePool;
    
    private void Start()
    {
        LoadUpgrades();
        LoadUpgradesIntoButtons();
    }

    private void LoadUpgrades() // This can be improved by selecting by tier or selecting unique elements.
    {
         upgradePool = new Upgrade[3];
         allUpgrades = Resources.LoadAll<Upgrade>("ScriptableObjects/Upgrades");
         for (int i = 0; i < 3; i++)
         {
             upgradePool[i] = allUpgrades[Random.Range(0, allUpgrades.Length)];
            Debug.Log(upgradePool[i].ToString());
         }
    }
    
    private void LoadUpgradesIntoButtons()
    {
        for (int i = 0; i < 3; i++)
        {
            SetupButton(upgradePool[i], upgradeButtons[i]);
        }
    }

    private void SetupButton(Upgrade upgradeData, UpgradeButtonUI button)
    {
       button.SetUpgrade(upgradeData);
    }

    public void Apply(UpgradeButtonUI upgradeButton)
    {
        var stats = PlayerStats.Instance;
        float value = 0;
        switch (upgradeButton.currentUpgrade.upgradeType)
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
        Debug.Log("Upgrade Selected = " + upgradeButton.currentUpgrade.ToString());
    }
}
