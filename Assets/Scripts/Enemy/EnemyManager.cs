using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
   private int enemiesAlive;

   public event Action onWaveCleared;
   
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
         onWaveCleared?.Invoke();
      }
   }
   public int GetEnemiesAlive() => enemiesAlive;
}
