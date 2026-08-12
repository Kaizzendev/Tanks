using System;
using ProceduralGeneration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GeneratorSceneManager : MonoBehaviour
{
    public LevelConfig defaultConfig;
    public ProceduralMap proceduralMap;

    private LevelConfig newConfig;
    
    [Header("UI")]
    public Slider mapSizeSliderX;
    public Slider mapSizeSliderY;
    public Slider enemyCountSlider;
    public Slider enemyMinDistanceSlider;
    public Slider objectSpacingSlider;

    private bool isInitializing;
    
    public TMP_InputField seedInputField;
    private void Start()
    {
        newConfig = Instantiate(defaultConfig);
        LoadConfigIntoInputs();
        Generate(defaultConfig);
    }

    private void LoadConfigIntoInputs()
    {
        isInitializing = true;
        
        enemyCountSlider.value = newConfig.enemyCount;
        mapSizeSliderX.value = newConfig.mapSize.x;
        mapSizeSliderY.value = newConfig.mapSize.y;
        objectSpacingSlider.value = newConfig.objectSpacing;
        seedInputField.text = newConfig.seed.ToString();
        enemyMinDistanceSlider.value = newConfig.enemyMinDistance;
        
        isInitializing = false;
    }
    
    public void ChangeConfiguration()
    {
        if (isInitializing) return;
        
        newConfig.enemyCount = (int)enemyCountSlider.value;
        newConfig.mapSize.x = (int)mapSizeSliderX.value;
        newConfig.mapSize.y = (int)mapSizeSliderY.value;
        newConfig.objectSpacing = (int)objectSpacingSlider.value;
        newConfig.seed = int.TryParse(seedInputField.text, out int seed) ? seed : Random.Range(0, 999999);
        newConfig.enemyMinDistance = (int)enemyMinDistanceSlider.value;
        
        Generate(newConfig);
    }

    private void Generate(LevelConfig config)
    {
        proceduralMap.Generate(
            config.seed, 
            config.biome, 
            config.mapSize, 
            config.objectSpacing, 
            config.noiseScale, 
            config.enemies,
            config.enemyMinDistance, 
            config.enemyCount,
            config.patrolPointprefab,
            config.patrolPointsMinDistance, 
            config.patrolPointsCount,
            false
            );
        Debug.Log("newConfig: " + config.ToString());
    }
}
