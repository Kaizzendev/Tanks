using System;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralGeneration
{
    public class EncounterGenerator: MonoBehaviour
    {
        [Header("NavMesh")] private NavMeshSurface _navSurface;

        [Header("Player")] private GameObject _playerPrefab;

        [SerializeField] private Encounter _encounter;
        
        
        public int seed = 12345;
        
        private GameObject _terrainPlane;
        
        
        private Transform _propsParent;
        private Transform _enemiesParent;
        private Transform _patrolPointsParent;
        
        private List<GameObject> _spawned = new List<GameObject>();
        

        private void Start()
        {
            Generate(_encounter);
        }

        public void Generate(Encounter encounter)
        {
            GeneratePlane(encounter.roomType.mapSize, encounter.biome.terrainMaterial, encounter.environmentSize);
            GeneratePlayableArea(encounter.roomType.wall, encounter.roomType.physicalWall, encounter.roomType.mapSize, encounter.environmentSize);
            GenerateParents();
            GenerateProps(
                encounter.roomType.mapSize,
                encounter.environmentSize,
                encounter.roomType.proceduralObjects,
                encounter.biome.proceduralObjects,
                encounter.roomType.objectSpacing,
                encounter.biome.objectSpacing,
                encounter.biome.objectNoiseScale, 
                encounter.roomType.objectNoiseScale
                );
        }

        private void GeneratePlane(Vector2 roomTypeMapSize, Material terrainMaterial,  float environmentSize)
        {
            _terrainPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            _terrainPlane.transform.SetParent(transform);
            _terrainPlane.transform.localPosition = Vector3.zero;
            _terrainPlane.transform.localScale = new Vector3((GetTotalMapSize(roomTypeMapSize,environmentSize).x) / 10f, 1f, GetTotalMapSize(roomTypeMapSize,environmentSize).y / 10f);

            _terrainPlane.GetComponent<Renderer>().material = terrainMaterial;
        }

        private Vector2 GetTotalMapSize(Vector2 mapSize, float environmentSize)
        {
            return new Vector2(mapSize.x + environmentSize, mapSize.y + environmentSize);
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
            _propsParent = new GameObject("Props").transform;
            _propsParent.SetParent(transform);

            _enemiesParent = new GameObject("Enemies").transform;
            _enemiesParent.SetParent(transform);

            _patrolPointsParent = new GameObject("Patrol points").transform;
            _patrolPointsParent.SetParent(transform);
        }

        private void GenerateProps(
            Vector2 playableMapSize,
            float environmentSize,
            List<ProceduralObject> playableObjects,
            List<ProceduralObject> environmentObjects,
            float playableObjectSpacing,
            float environmentObjectSpacing,
            float playableNoiseScale,
            float environmentNoiseScale)
        {
            Random.InitState(seed);

            Vector2 mapTotalSize = GetTotalMapSize(playableMapSize, environmentSize);

            List<Vector2> points = PoissonDiskSampler.Generate(
                playableObjectSpacing,
                environmentObjectSpacing,
                mapTotalSize,
                playableMapSize,
                30
            );

            float noiseScale;
            List<ProceduralObject> objects = new List<ProceduralObject>();

            foreach (Vector2 p in points)
            {
                float x = p.x - mapTotalSize.x / 2f;
                float z = p.y - mapTotalSize.y / 2f;

                noiseScale = playableNoiseScale;
                objects = playableObjects;
                
                Vector3 size = new Vector3(playableMapSize.x, 0, playableMapSize.y);
                bool isInside = new Bounds(Vector3.zero, size).Contains(new Vector3(x, 0, z));
                
                if (!isInside) 
                {
                    noiseScale = environmentNoiseScale;
                    objects = environmentObjects;
                }
                
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

                GameObject inst = Instantiate(obj.proceduralObject, pos, rot, _propsParent);

                // variación de escala
                float scale = Random.Range(obj.scaleRange.x, obj.scaleRange.y);
                inst.transform.localScale *= scale;

                _spawned.Add(inst);

            }
        }
        
        private static class PoissonDiskSampler
        {
            public static List<Vector2> Generate(float playableRadius, float environmentRadius, Vector2 regionSize, Vector2 playableSize, int rejectionSamples)
            {
                float playableCellSize = playableRadius / Mathf.Sqrt(2);
                float environmentCellSize = environmentRadius / Mathf.Sqrt(2);

                int playableGridW = Mathf.CeilToInt(regionSize.x / playableCellSize);
                int playableGridH = Mathf.CeilToInt(regionSize.y / playableCellSize);
                
                int environmentGridW = Mathf.CeilToInt(regionSize.x / environmentCellSize);
                int environmentGridH = Mathf.CeilToInt(regionSize.y / environmentCellSize);

                int[,] playableGrid = new int[playableGridW, playableGridH];
                
                int[,] environmentGrid = new int[environmentGridW, environmentGridH];

                List<Vector2> points = new List<Vector2>();
                List<Vector2> spawnPoints = new List<Vector2>();

                spawnPoints.Add(regionSize / 2);


                float cellSize;
                float radius;
                int[,] grid;
                
                while (spawnPoints.Count > 0)
                {
                    int spawnIndex = Random.Range(0, spawnPoints.Count);
                    Vector2 spawnCenter = spawnPoints[spawnIndex];
                    bool accepted = false;

                    for (int i = 0; i < rejectionSamples; i++)
                    {
                        
                        cellSize = playableCellSize;
                        radius = playableRadius;
                        grid = playableGrid;

                        Vector3 size = new Vector3(playableSize.x, 0, playableSize.y);
                        bool isInside = new Bounds(new Vector3(regionSize.x /2, 0, regionSize.y /2), size).Contains(new Vector3(spawnPoints[spawnIndex].x, 0, spawnPoints[spawnIndex].y));
                        
                        if (!isInside)
                        {
                            cellSize = environmentCellSize;
                            radius = environmentRadius;
                            grid = environmentGrid;
                        }
                        
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