using UnityEngine;

namespace Enemy.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "AI Action", menuName = "AI/Actions/Patrol")]
    public class UtilitySystemActionPatrol: UtilitySystemAction
    {
        public override void ExecuteAction(AIContext context)
        {
            context.enemy.Patrol();
        }
    }
}