using UnityEngine;

[CreateAssetMenu(fileName = "ProceduralObject", menuName = "Scriptable Objects/ProceduralObject")]
public class ProceduralObject : ScriptableObject
{
    public GameObject proceduralObject;

    [Header("Density")]
    public float minDensity = 0f;
    public float maxDensity = 1f;
    
    [Header("Probability")]
    public float probability = 0f;
    
    [Header("Scale probability")]
    public Vector2 scaleRange = new Vector2(1f, 1f);
}
