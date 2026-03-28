using Player;
using TMPro;
using UnityEngine;

public class StatManagerUI : MonoBehaviour
{
    public TextMeshProUGUI HpText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI enemiesText;
    
    void Start()
    {
        
        
    }

    void Update()
    {
        enemiesText.text = "Enemies left: " + GameManager.Instance.enemyManager.GetEnemiesAlive();
        levelText.text = "Level: " + LevelManager.Instance.currentLevel;
        HpText.text = "HP: " + PlayerStats.Instance.currentHealth;
    }
}
