using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UpgradeManager : MonoBehaviour
{
    
    private UpgradeButtonUI upgradeButtonUI;
    [Header("UI References")] 
    public UpgradeButtonUI[] upgradeButtons;
    public RectTransform upgradeButtonsContainer;

    private Upgrade[] allUpgrades;
    private Upgrade[] upgradePool;

    private void OnEnable()
    {
        GameManager.Instance.onStateChanged += OnGameStateChanged;
    }


    private void OnDisable()
    {
        GameManager.Instance.onStateChanged -= OnGameStateChanged;
    }
    
    private void OnGameStateChanged(GameManager.GameState state)
    {
        if (state == GameManager.GameState.reward)
        {
            LoadUpgrades();
            LoadUpgradesIntoButtons();
            upgradeButtonsContainer.gameObject.SetActive(true);
        }
    }

    private void Start()
    {
        LoadUpgrades();
        LoadUpgradesIntoButtons();
    }

    private void LoadUpgrades() // This can be improved by selecting by tier or selecting unique elements.
    {
         upgradePool = new Upgrade[3];
         allUpgrades = Resources.LoadAll<Upgrade>("ScriptableObjects/Upgrades");

         for (int i = 0; i < allUpgrades.Length; i++)
         {
             int randomIndex = Random.Range(i, allUpgrades.Length);
             Upgrade tempUpgrade = allUpgrades[i];
             allUpgrades[i] = allUpgrades[randomIndex];
             allUpgrades[randomIndex] = tempUpgrade;
         }
         
         for (int i = 0; i < 3; i++)
         {
             upgradePool[i] = allUpgrades[i];
            //Debug.Log(upgradePool[i].ToString());
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
        float value = upgradeButton.currentUpgrade.value;
        switch (upgradeButton.currentUpgrade.upgradeType)
        {
            case UpgradeType.Damage:
                PlayerStats.Instance.damage += value;
                break;
            case UpgradeType.MaxHealth:
                PlayerStats.Instance.maxHealth += value;
                PlayerStats.Instance.UpgradeHP();
                break;
            case UpgradeType.MoveSpeed:
                PlayerStats.Instance.moveSpeed += value;
                break;
            case UpgradeType.AttackSpeed:
                PlayerStats.Instance.attackSpeed -= value;
                break;
            case UpgradeType.CriticChance:
                PlayerStats.Instance.criticChance += value;
                break;
            case UpgradeType.CriticDamage:
                PlayerStats.Instance.criticMultiplier += value;
                break;
        }

        ReturnPlayState();
        Debug.Log("Upgrade Selected = " + upgradeButton.currentUpgrade.ToString());

    }

    private void ReturnPlayState()
    {
        upgradeButtonsContainer.gameObject.SetActive(false);
        GameEvents.onUpgradeButtonClicked?.Invoke();
    }
    
}
