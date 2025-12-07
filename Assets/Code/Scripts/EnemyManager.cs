using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   private int enemiesAlive;

   private void OnEnable()
   {
      EnemyEvents.OnEnemySpawned += RegisterEnemy;
      EnemyEvents.OnEnemyDied += UnRegisterEnemy;
   }

   private void OnDisable()
   {
      EnemyEvents.OnEnemySpawned -= RegisterEnemy;
      EnemyEvents.OnEnemyDied -= UnRegisterEnemy;
   }

   public void RegisterEnemy()
   {
      enemiesAlive++;
   }

   public void UnRegisterEnemy()
   {
      enemiesAlive--;
      if (enemiesAlive <= 0)
      {
         GameManager.Instance.ChangeState(GameManager.GameState.reward);
      }
   }
#if UNITY_EDITOR
   [ContextMenu("Count Enemies")]
#endif
   public void GetEnemiesAlive() => print(enemiesAlive);
}
