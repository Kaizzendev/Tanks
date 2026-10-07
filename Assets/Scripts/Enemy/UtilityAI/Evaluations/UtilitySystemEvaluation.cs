using UnityEngine;

namespace Enemy.UtilityAI.Evaluations
{
    public abstract class UtilitySystemEvaluation: ScriptableObject
    {
        [SerializeField] internal float weight;
        public abstract float GetScore(AIContext context);
    }
}