using UnityEngine;

namespace Enemy.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "AI Action", menuName = "AI/Actions/Chase")]
    public class UtilitySystemActionChase: UtilitySystemAction
    {
        public override void ExecuteAction(AIContext context)
        {
            context.enemy.Chase();
        }
    }
}