using ProceduralGeneration;
using UnityEngine;

namespace Map
{
    [CreateAssetMenu(fileName = "Encounter", menuName = "Scriptable Objects/Map/EncountersRules")]
    public class EncounterTypeGenerationRule : ScriptableObject
    {
        [Header("Encounter Type")]
        [SerializeField] internal EncounterType EncounterType;
        
        [Header("Layers")] 
        [SerializeField] [Range(0.1f,1)] internal float MinMapProgress = 0.1f;
        [SerializeField] [Range(0.1f,1)] internal float MaxMapProgress = 1f;
        
        [Header("Encounter probability")]
        [SerializeField] internal AnimationCurve AnimationCurve;
        
    }
}