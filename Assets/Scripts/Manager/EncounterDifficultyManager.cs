using System;
using Map;
using ProceduralGeneration;
using UnityEngine;

namespace Manager
{
    public class EncounterDifficultyManager: MonoBehaviour
    {

        private string _nextLevelName;
        
        private void Start()
        {
            MapManager.Instance.OnNodeReached += SetEncounter;
        }
        
        private void OnDisable()
        {
            MapManager.Instance.OnNodeReached -= SetEncounter;
        }

        private void SetEncounter(MapNode currentNode)
        {
            EncounterType encounterType = currentNode.EncounterType;
            
            Encounter baseEncounterData = Resources.Load<Encounter>($"ScriptableObjects/Encounters/{encounterType.ToString()}");

            switch (baseEncounterData.encounterType)
            {
                case EncounterType.Choice:
                    _nextLevelName = "ChoiceLevel";
                    break;
                case EncounterType.Shop:
                    _nextLevelName = "ShopLevel";
                    break;
                default:
                    _nextLevelName = "Level";
                    SetEncounterDifficulty(currentNode, baseEncounterData);
                    break;

            }
            
            LoadNextLevel(_nextLevelName);
        }

        private void SetEncounterDifficulty(MapNode currentNode, Encounter encounterData)
        {
            //TODO: Change Difficulty based on layer, encounter type and progression
            
            Encounter currentEncounterData = Instantiate(encounterData);
            Room currentRoomData = Instantiate(encounterData.roomType);
            Biome currentBiomeData = Instantiate(encounterData.biome);
            
            currentEncounterData.roomType = currentRoomData;
            currentEncounterData.biome = currentBiomeData;

            GameManager.Instance.CurrentEncounterData = currentEncounterData;
        }

        private void LoadNextLevel(string levelName)
        {
            SceneLoader.LoadScene(levelName);
        }

    }
}