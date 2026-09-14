using System;
using Map;
using ProceduralGeneration;
using UnityEngine;

namespace Manager
{
    public class EncounterDifficultyManager: MonoBehaviour
    {
        private void OnDisable()
        {
            MapManager.Instance.OnNodeReached -= SetEncounterDifficulty;
        }

        private void Start()
        {
            MapManager.Instance.OnNodeReached += SetEncounterDifficulty;
        }

        private void SetEncounterDifficulty(MapNode currentNode)
        {
            EncounterType encounterType = currentNode.EncounterType;
            
            Encounter encounterData = Resources.Load<Encounter>($"ScriptableObjects/Encounters/{encounterType.ToString()}");
            
            //Cambiar dificultad 
            
            GameManager.Instance.CurrentEncounterData = encounterData;
            
            SceneManager.LoadLevel();
        }
        
    }
}