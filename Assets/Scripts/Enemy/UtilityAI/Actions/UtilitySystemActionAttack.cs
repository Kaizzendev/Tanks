using Enemy.UtilityAI.Evaluations;
using UnityEngine;

namespace Enemy.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "AI Action", menuName = "AI/Actions/Attack")]
    public class UtilitySystemActionAttack: UtilitySystemAction
    {
        public override void ExecuteAction(AIContext context)
        {
            context.enemy.Attack();
        }
    }
}