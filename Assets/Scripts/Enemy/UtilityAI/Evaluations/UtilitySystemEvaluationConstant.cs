using Enemy.UtilityAI.Actions;
using UnityEngine;

namespace Enemy.UtilityAI.Evaluations
{
    [CreateAssetMenu(fileName = "AI Evaluation", menuName = "AI/Evaluations/Constant")]
    public class UtilitySystemEvaluationConstant: UtilitySystemEvaluation
    {
        [SerializeField] private float _constant;
        public override float GetScore(AIContext context)
        {
            return _constant;
        }
    }
}