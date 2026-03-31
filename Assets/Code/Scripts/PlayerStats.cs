using System;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public event Action onPlayerChangeHP;
        public static PlayerStats Instance;

        [Header("Health Stats")]
        [SerializeField] internal float maxHealth = 100f;
        [SerializeField] internal float currentHealth;

        [Header("Damage Stats")]
        [SerializeField] internal float damage = 100f;
        [SerializeField] internal float attackSpeed = 1f;
        [SerializeField] internal float criticChance = 0f;
        [SerializeField] internal float criticMultiplier = 2f;
        
        [Header("Movement Stats")]
        [SerializeField] internal float maxMoveSpeed = 14f;
        [SerializeField] internal float moveSpeed = 5f;
        [SerializeField] internal float rotationSpeed = 2f;
        

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            onPlayerChangeHP?.Invoke();
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            currentHealth += amount;
        }

        public void UpgradeHP()
        {
            currentHealth = maxHealth;
            onPlayerChangeHP?.Invoke();
        }

        private void Die()
        {
            GameManager.Instance.ChangeState(GameManager.GameState.gameOver);
        }
    }
}