using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralGeneration
{
    public class EncounterGenerator: MonoBehaviour
    {
        [Header("NavMesh")] public NavMeshSurface navSurface;

        [Header("Player")] public GameObject playerPrefab;

        [SerializeField] private Encounter _encounter;
        
        
        public int seed = 12345;
        
        private GameObject terrainPlane;
        
        
        private Transform propsParent;
        private Transform enemiesParent;
        private Transform patrolPointsParent;
        
        private List<GameObject> spawned = new List<GameObject>();
        

        private void Start()
        {
            Generate(_encounter);
        }

        public void Generate(Encounter encounter)
        {
            GeneratePlane(encounter.roomType.mapSize, encounter.biome.terrainMaterial, encounter.environmentSize);
            GeneratePlayableArea(encounter.roomType.wall, encounter.roomType.physicalWall, encounter.roomType.mapSize, encounter.environmentSize);
            GenerateParents();
            GeneratePlayableProps(encounter.roomType.proceduralObjects, encounter.roomType.objectSpacing, encounter.roomType.mapSize, encounter.roomType.objectNoiseScale);
        }

        private void GeneratePlane(Vector2 roomTypeMapSize, Material terrainMaterial,  float environmentSize)
        {
            terrainPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            terrainPlane.transform.SetParent(transform);
            terrainPlane.transform.localPosition = Vector3.zero;
            terrainPlane.transform.localScale = new Vector3((roomTypeMapSize.x + environmentSize) / 10f, 1f, (roomTypeMapSize.y + environmentSize) / 10f);

            terrainPlane.GetComponent<Renderer>().material = terrainMaterial;
        }


        private void GeneratePlayableArea(GameObject wall, bool physicalWall, Vector2 roomTypeMapSize, float environmentSize)
        {
            if (physicalWall)
            {
                environmentSize = 0;
            }
            
            Vector3 topLeftCorner = new Vector3(-roomTypeMapSize.x - environmentSize,0,-roomTypeMapSize.y - environmentSize) / 2;
            Vector3 topRightCorner = new Vector3(roomTypeMapSize.x + environmentSize,0,-roomTypeMapSize.y - environmentSize) / 2;
            Vector3 bottomLeftCorner = new Vector3(-roomTypeMapSize.x - environmentSize,0,roomTypeMapSize.y + environmentSize) / 2;
            Vector3 bottomRightCorner = new Vector3(roomTypeMapSize.x + environmentSize,0,roomTypeMapSize.y + environmentSize) / 2;
            
            GenerateWalls(topLeftCorner, topRightCorner, wall);
            GenerateWalls(topRightCorner, bottomRightCorner, wall);
            GenerateWalls(bottomRightCorner, bottomLeftCorner, wall);
            GenerateWalls(bottomLeftCorner, topLeftCorner, wall);
            
        }

        private void GenerateWalls(Vector3 start, Vector3 end, GameObject wall )
        {
            
            Vector3 direction = (end - start).normalized;
            float distance = Vector3.Distance(start, end);
            int wallCount = Mathf.FloorToInt(distance / 2);


            for (int i = 0; i < wallCount; i++)
            {
                Vector3 position = start + direction * (i * 2);
                
                Instantiate(wall, position, Quaternion.identity);
            }
        }
        
        private void GenerateParents()
        {
            propsParent = new GameObject("Props").transform;
            propsParent.SetParent(transform);

            enemiesParent = new GameObject("Enemies").transform;
            enemiesParent.SetParent(transform);

            patrolPointsParent = new GameObject("Patrol points").transform;
            patrolPointsParent.SetParent(transform);
        }

        private void GeneratePlayableProps(List<ProceduralObject> objects, float objectSpacing, Vector2 mapSize, float noiseScale)
        {
            Random.InitState(seed);

            List<Vector2> points = PoissonDiskSampler.Generate(
                objectSpacing,
                mapSize,
                30
            );

            foreach (Vector2 p in points)
            {
                float x = p.x - mapSize.x / 2f;
                float z = p.y - mapSize.y / 2f;

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

                GameObject inst = Instantiate(obj.proceduralObject, pos, rot, propsParent);

                // variación de escala
                float scale = Random.Range(obj.scaleRange.x, obj.scaleRange.y);
                inst.transform.localScale *= scale;

                spawned.Add(inst);

            }
        }
        
        private void GenerateEnvironmentProps(List<ProceduralObject> objects, float objectSpacing, Vector2 mapSize, float noiseScale)
        {

            List<Vector2> points = PoissonDiskSampler.Generate(
                objectSpacing,
                mapSize,
                30
            );

            foreach (Vector2 p in points)
            {
                float x = p.x - mapSize.x / 2f;
                float z = p.y - mapSize.y / 2f;

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

                GameObject inst = Instantiate(obj.proceduralObject, pos, rot, propsParent);

                // variación de escala
                float scale = Random.Range(obj.scaleRange.x, obj.scaleRange.y);
                inst.transform.localScale *= scale;

                spawned.Add(inst);

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
}