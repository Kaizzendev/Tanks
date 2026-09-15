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
            
            Encounter baseEncounterData = Resources.Load<Encounter>($"ScriptableObjects/Encounters/{encounterType.ToString()}");
            
            Encounter currentEncounterData = Instantiate(baseEncounterData);
            Room currentRoomData = Instantiate(baseEncounterData.roomType);
            Biome currentBiomeData = Instantiate(baseEncounterData.biome);
            
            currentEncounterData.roomType = currentRoomData;
            currentEncounterData.biome = currentBiomeData;
            
            
            //TODO: Change Difficulty based on layer, encounter type and progression

            GameManager.Instance.CurrentEncounterData = currentEncounterData;
            
            SceneLoader.LoadLevel();
        }
        
    }
}