using UnityEngine;

namespace Enemy.UtilityAI.Actions
{
    [CreateAssetMenu(fileName = "AI Action", menuName = "AI/Actions/Hide")]
    public class UtilitySystemActionHide: UtilitySystemAction
    {
        public override void ExecuteAction(AIContext context)
        {
            Debug.Log("Me quiero esconder");
            //context.enemy.Hide();
        }
    }
}