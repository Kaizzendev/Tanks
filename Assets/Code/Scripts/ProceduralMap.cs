using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProceduralMap : MonoBehaviour
{
    [Header("Map Settings")]
    public float mapSize = 100f;
    public float objectSpacing = 5f;
    public int seed = 12345;

    [Header("Noise Settings")]
    public float noiseScale = 0.5f;

    [Header("Objects")]
    public List<ProceduralObject> objects = new List<ProceduralObject>();

    private List<GameObject> spawned = new List<GameObject>();

    public GameObject enemy;
    
#if UNITY_EDITOR
    [ContextMenu("Generate Map")]
#endif
    public void GenerateContextMenu()
    {
        ClearMap();
        Generate();
    }


    public void ClearMap()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }
    public void Generate()
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
            print(spawned);
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
