using System;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy.UtilityAI.Evaluations
{
    [CreateAssetMenu(fileName = "AI Evaluation", menuName = "AI/Evaluations/Fusion")]
    public class UtilitySystemEvaluationFusion: UtilitySystemEvaluation
    {
        
        [SerializeField] private List<UtilitySystemEvaluation> _evaluations;
        public override float GetScore(AIContext context)
        {
            float totalScore = 0;
            float totalWeight = 0;
            
            foreach (var evaluation in _evaluations)
            {
                totalScore += evaluation.GetScore(context) * evaluation.weight;
                totalWeight += evaluation.weight;
            }

            if (totalWeight == 0)
            {
                return 0f;
            }
            
            return totalScore / totalWeight;
        }
    }
}