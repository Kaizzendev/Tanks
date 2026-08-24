using System;
using Manager;
using Player;
using TMPro;
using UnityEngine;

public class StatManagerUI : MonoBehaviour
{
    public UpgradeManager upgradeManager;
    
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI enemiesText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI criticChanceText;
    public TextMeshProUGUI criticMultiplierText;
    public TextMeshProUGUI attackSpeedText;
    
    public TextMeshProUGUI openStatsText;
    public GameObject advanceStats;

    private bool isAdvancedStatsOpen = false;

    private void OnEnable()
    {
        
        GameEvents.onUpgradeButtonClicked += UpdateStats;
        PlayerStats.Instance.onPlayerChangeHP += UpdateHP;
    }

    private void OnDisable()
    {
        GameEvents.onUpgradeButtonClicked -= UpdateStats;
        PlayerStats.Instance.onPlayerChangeHP -= UpdateHP;
    }

    void Start()
    {
        advanceStats.SetActive(false);
        UpdateStats();
        UpdateHP();
    }

    void UpdateHP()
    {
        HpText.text = "HP: " + PlayerStats.Instance.currentHealth ;
    }

    void UpdateStats()
    {
        speedText.text = "Speed: " + PlayerStats.Instance.moveSpeed;
        damageText.text = "Damage: " + PlayerStats.Instance.damage;
        criticChanceText.text = "Critic: " + PlayerStats.Instance.criticChance;
        criticMultiplierText.text = "Critic mult: " + PlayerStats.Instance.criticMultiplier;
        attackSpeedText.text = "AT speed: " + PlayerStats.Instance.attackSpeed;
    }
    void Update()
    {
        enemiesText.text = "Enemies left: " + GameManager.Instance.enemyManager.GetEnemiesAlive();
        levelText.text = "Level: " + LevelManager.Instance.currentLevel;
        
        
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            isAdvancedStatsOpen = !isAdvancedStatsOpen;
            
            advanceStats.SetActive(isAdvancedStatsOpen);
            openStatsText.gameObject.SetActive(!isAdvancedStatsOpen);
        }
    }
}
