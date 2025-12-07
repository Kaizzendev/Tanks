using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        public static PlayerStats Instance;

        [Header("Health Stats")]
        [SerializeField] internal float maxHealth = 100f;
        [SerializeField] internal float currentHealth;

        [Header("Damage Stats")]
        [SerializeField] internal float damage = 100f;
        [SerializeField] internal float attackSpeed = 0.5f;
        [SerializeField] internal float criticChance = 0f;
        [SerializeField] internal float criticMultiplier = 2f;
        
        [Header("Movement Stats")]
        [SerializeField] internal float maxMoveSpeed = 14f;
        [SerializeField] internal float moveSpeed = 5f;
        [SerializeField] internal float rotationSpeed = 2f;
        

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            GameManager.Instance.ChangeState(GameManager.GameState.gameOver);
        }
    }
}