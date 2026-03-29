using Player;
using TMPro;
using UnityEngine;

public class StatManagerUI : MonoBehaviour
{
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

    void Start()
    {
        advanceStats.SetActive(false);
    }
    void Update()
    {
        enemiesText.text = "Enemies left: " + GameManager.Instance.enemyManager.GetEnemiesAlive();
        levelText.text = "Level: " + LevelManager.Instance.currentLevel;
        HpText.text = "HP: " + PlayerStats.Instance.currentHealth;
        speedText.text = "Speed: " + PlayerStats.Instance.moveSpeed;
        damageText.text = "Damage: " + PlayerStats.Instance.damage;
        criticChanceText.text = "Critic Chance: " + PlayerStats.Instance.criticChance;
        criticMultiplierText.text = "Critic Multiplier: " + PlayerStats.Instance.criticMultiplier;
        attackSpeedText.text = "AT speed: " + PlayerStats.Instance.attackSpeed;
        
        if (Input.GetKeyDown(KeyCode.F9))
        {
            isAdvancedStatsOpen = !isAdvancedStatsOpen;
            
            advanceStats.SetActive(isAdvancedStatsOpen);
            openStatsText.gameObject.SetActive(!isAdvancedStatsOpen);
        }
    }
}
