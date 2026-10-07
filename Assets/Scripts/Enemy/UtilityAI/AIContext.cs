namespace Enemy.UtilityAI
{
    public class AIContext
    {
        public EnemyController enemy;
        public bool isPlayerVisible;
        public bool isReloading;
        public bool isNearShoots;
        public float distanceToPlayer;
        public float currentHealth;
        public float detectionRange;
        public float maxHealth;
    }
}