using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralMap : MonoBehaviour
{
    [Header("Map Settings")]
    public float mapSize = 100f;
    public Material terrainMaterial;
    public float objectSpacing = 5f;
    public int seed = 12345;

    [Header("Noise Settings")]
    public float noiseScale = 0.05f;

    [Header("Objects")]
    public List<ProceduralObject> objects = new List<ProceduralObject>();
    public float objectDensity = 0.3f;

    [Header("NavMesh")]
    public NavMeshSurface navSurface;
    
    
    [Header("Enemy Settings")]
    public GameObject enemy;
    public int enemyCount = 10;
    public float enemyMinDistance = 8f; 
    
    private GameObject terrainPlane;
    private List<GameObject> spawned = new List<GameObject>();
    
    
#if UNITY_EDITOR
    [ContextMenu("Generate Map")]
#endif
    public void GenerateContextMenu()
    {
        Generate(1);
    }


    public void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private void Awake()
    {
        Generate(1);
    }

    public void Generate(int difficulty)
    {
        ClearMap();
        GeneratePlane();
        GenerateProps();
        BakeNavMesh();
        spawnEnemies();
    }

    private void GeneratePlane()
    {
        terrainPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        terrainPlane.transform.SetParent(transform);
        terrainPlane.transform.localPosition = Vector3.zero;
        terrainPlane.transform.localScale = new Vector3(mapSize / 10f, 1f, mapSize / 10f);
        
        terrainPlane.GetComponent<Renderer>().material = terrainMaterial;
    }
    
    public void GenerateProps()
    {
        Random.InitState(seed);
        
        List<Vector2> points = PoissonDiskSampler.Generate(
            objectSpacing,
            new Vector2(mapSize, mapSize),
            30
        );

        foreach (Vector2 p in points)
        {
            float x = p.x - mapSize / 2f;
            float z = p.y - mapSize / 2f;

            float density = Mathf.PerlinNoise(
                (p.x + seed) * noiseScale,
                (p.y + seed) * noiseScale
            );

            
            // seleccionar objeto según reglas
            ProceduralObject obj = ObjectRuleEngine.SelectObject(density, objects);

            if (obj == null)
                continue;

            // instanciar
            Vector3 pos = new Vector3(x, 0f, z);
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            GameObject inst = Instantiate(obj.proceduralObject, pos, rot, transform);

            // variación de escala
            float scale = Random.Range(obj.scaleRange.x, obj.scaleRange.y);
            inst.transform.localScale *= scale;

            spawned.Add(inst);

        }
    }
    
    private void BakeNavMesh()
    {
        if (navSurface != null)
            navSurface.BuildNavMesh();
    }

    private void spawnEnemies()
    {
        if (enemy == null) return;

        // Creamos posiciones Poisson para enemigos
        List<Vector2> points = PoissonDiskSampler.Generate(
            enemyMinDistance,
            new Vector2(mapSize, mapSize),
            20
        );

        // Limitamos a enemyCount
        for (int i = 0; i < Mathf.Min(enemyCount, points.Count); i++)
        {
            Vector2 p = points[i];
            float x = p.x - mapSize / 2f;
            float z = p.y - mapSize / 2f;

            Vector3 pos = new Vector3(x, 5f, z);

            Instantiate(enemy, pos, Quaternion.identity, transform);
        }
    }

    private static class PoissonDiskSampler
    {
        public static List<Vector2> Generate(float radius, Vector2 regionSize, int rejectionSamples)
        {
            float cellSize = radius / Mathf.Sqrt(2);

            int gridW = Mathf.CeilToInt(regionSize.x / cellSize);
            int gridH = Mathf.CeilToInt(regionSize.y / cellSize);

            int[,] grid = new int[gridW, gridH];

            List<Vector2> points = new List<Vector2>();
            List<Vector2> spawnPoints = new List<Vector2>();

            spawnPoints.Add(regionSize / 2);

            while (spawnPoints.Count > 0)
            {
                int spawnIndex = Random.Range(0, spawnPoints.Count);
                Vector2 spawnCenter = spawnPoints[spawnIndex];
                bool accepted = false;

                for (int i = 0; i < rejectionSamples; i++)
                {
                    float angle = Random.value * Mathf.PI * 2;
                    float dist = Random.Range(radius, radius * 2);
                    Vector2 candidate = spawnCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;

                    if (IsValid(candidate, regionSize, cellSize, radius, points, grid))
                    {
                        points.Add(candidate);
                        spawnPoints.Add(candidate);
                        grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize)] = points.Count;
                        accepted = true;
                        break;
                    }
                }

                if (!accepted)
                    spawnPoints.RemoveAt(spawnIndex);
            }

            return points;
        }


        private static bool IsValid(
            Vector2 candidate, Vector2 region, float cellSize, float radius,
            List<Vector2> points, int[,] grid)
        {
            if (candidate.x < 0 || candidate.x >= region.x || candidate.y < 0 || candidate.y >= region.y)
                return false;

            int cellX = (int)(candidate.x / cellSize);
            int cellY = (int)(candidate.y / cellSize);

            int searchStartX = Mathf.Max(0, cellX - 2);
            int searchEndX = Mathf.Min(grid.GetLength(0) - 1, cellX + 2);
            int searchStartY = Mathf.Max(0, cellY - 2);
            int searchEndY = Mathf.Min(grid.GetLength(1) - 1, cellY + 2);

            for (int x = searchStartX; x <= searchEndX; x++)
            {
                for (int y = searchStartY; y <= searchEndY; y++)
                {
                    int pointIndex = grid[x, y] - 1;
                    if (pointIndex != -1)
                    {
                        float sqrDist = (candidate - points[pointIndex]).sqrMagnitude;
                        if (sqrDist < radius * radius)
                            return false;
                    }
                }
            }

            return true;
        }
    }

}
