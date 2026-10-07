using UnityEngine;

namespace Enemy.UtilityAI.Evaluations
{
    [CreateAssetMenu(fileName = "AI Evaluation", menuName = "AI/Evaluations/Function")]
    public class UtilitySystemEvaluationFunction: UtilitySystemEvaluation
    {
        
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private ValueToEvaluate _valueToEvaluate;
        public override float GetScore(AIContext context)
        {
            return _animationCurve.Evaluate(GetUtilityValue(context));
        }

        private float GetUtilityValue(AIContext context)
        {
            float value = 0f;
            
            switch (_valueToEvaluate)
            {
                case ValueToEvaluate.DistanceToPlayer:
                    value = context.distanceToPlayer / context.detectionRange;
                    break;
                case ValueToEvaluate.HealthPercentage:
                    value = context.currentHealth / context.maxHealth;
                    break;
            }

            return Mathf.Clamp01(value);
        }
    }

    public enum ValueToEvaluate
    {
        DistanceToPlayer,
        HealthPercentage,
    }
}