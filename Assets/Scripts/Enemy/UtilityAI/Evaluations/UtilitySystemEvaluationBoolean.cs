using UnityEngine;

namespace Enemy.UtilityAI.Evaluations
{
    [CreateAssetMenu(fileName = "AI Evaluation", menuName = "AI/Evaluations/Boolean")]
    public class UtilitySystemEvaluationBoolean: UtilitySystemEvaluation
    {
        [SerializeField] private BooleanType _booleanType;
        public override float GetScore(AIContext context)
        {
            bool value = false;
            switch (_booleanType)
            {
                case BooleanType.PlayerVisible:
                    value = context.isPlayerVisible;
                    break;
                case BooleanType.Reloading:
                    value = context.isReloading;
                    break;
                case BooleanType.NearShoots:
                    value = context.isNearShoots;
                    break;
            }

            return value ? 1f : 0f;
        }
    }

    public enum BooleanType
    {
        PlayerVisible,
        Reloading,
        NearShoots
    }
}