using Enemy.UtilityAI.Evaluations;
using UnityEngine;

namespace Enemy.UtilityAI.Actions
{
    public abstract class UtilitySystemAction: ScriptableObject
    {
        [SerializeField] private UtilitySystemEvaluation _utilitySystemEvaluation;
        [SerializeField] internal string _actionName;

        internal float EvaluateScore(AIContext context)
        {
            return _utilitySystemEvaluation.GetScore(context);
        }
        
        public abstract void ExecuteAction(AIContext context);
    }
}