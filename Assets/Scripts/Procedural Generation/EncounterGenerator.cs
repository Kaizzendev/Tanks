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

        [Header("Player")] [SerializeField] private GameObject _playerPrefab;

        [SerializeField] private Encounter _encounter;

        [SerializeField] private float _safeSpawnDistance = 6;
        [SerializeField] private float _playerSafeSpawnEnemiesDistance = 15;
        [Header("Seed")] public int seed = 12345;
        
        private GameObject _terrainPlane;
        
        
        private Transform _propsParent;
        private Transform _enemiesParent;
        private Transform _patrolPointsParent;
        private Transform _wallsParent;
        

        private Vector2 _playerSpawnPosition;
        
        private List<GameObject> _spawned = new List<GameObject>();
        

        private void Start()
        {
            Generate(_encounter);
        }
        
#if UNITY_EDITOR
        [ContextMenu("Generate Map")]
#endif
        public void GenerateContextMenu()
        {
            Generate(_encounter);
        }

        public void Generate(Encounter encounter)
        {
            ClearMap();
            GenerateSeed();
            GeneratePlane(encounter.roomType.mapSize, encounter.biome.terrainMaterial, encounter.environmentSize);
            GenerateParents();
            GeneratePlayableArea(encounter.roomType.wall, encounter.roomType.physicalWall, encounter.roomType.mapSize, encounter.environmentSize);
            GenerateProps(
                encounter.roomType.mapSize,
                encounter.environmentSize,
                encounter.roomType.proceduralObjects,
                encounter.biome.proceduralObjects,
                encounter.roomType.objectSpacing,
                encounter.biome.objectSpacing,
                encounter.biome.objectNoiseScale, 
                encounter.roomType.objectNoiseScale,
                encounter.roomType.wall
                );

            FindPlayerSpawnPosition(encounter.roomType.mapSize, encounter.roomType.wall);
            FindEnemiesSpawnPosition(encounter.roomType.mapSize, encounter.roomType.wall, encounter.enemies, encounter.enemyCount);
        }
        
        private void ClearMap()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void GenerateSeed()
        {
            Random.InitState(seed);
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
            return new Vector2(mapSize.x + environmentSize , mapSize.y + environmentSize);
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
            int wallCount = Mathf.FloorToInt(distance / (wall.transform.localScale.x * wall.GetComponent<BoxCollider>().size.x) );


            for (int i = 0; i < wallCount; i++)
            {
                Vector3 position = start + direction * (i * (wall.transform.localScale.x * wall.GetComponent<BoxCollider>().size.x));
                
                Instantiate(wall, position, Quaternion.identity, _wallsParent);
            }
        }
        
        private void GenerateParents()
        {
            _propsParent = new GameObject("Props").transform;
            _propsParent.SetParent(transform);
            
            _wallsParent = new GameObject("Walls").transform;
            _wallsParent.SetParent(transform);

            _enemiesParent = new GameObject("Enemies").transform;
            _enemiesParent.SetParent(transform);

            _patrolPointsParent = new GameObject("Patrol points").transform;
            _patrolPointsParent.SetParent(transform);
        }

        private static bool IsPointInside(Vector3 center, Vector3 size, Vector3 point)
        {
            return new Bounds(center, size).Contains(point);
        }

        private static Vector3 GetSafePlayableArea(Vector2 playableArea, GameObject wall)
        {
            return new Vector3(playableArea.x + 
                               (wall.GetComponent<BoxCollider>().size.x * wall.transform.localScale.x) + 2f, 0, 
                playableArea.y + (wall.GetComponent<BoxCollider>().size.x * wall.transform.localScale.x) + 2f) ;
        }

        private void GenerateProps(
            Vector2 playableMapSize,
            float environmentSize,
            List<ProceduralObject> playableObjects,
            List<ProceduralObject> environmentObjects,
            float playableObjectSpacing,
            float environmentObjectSpacing,
            float playableNoiseScale,
            float environmentNoiseScale,
            GameObject wall
            )
        {
            Vector2 mapTotalSize = GetTotalMapSize(playableMapSize, environmentSize);

            List<Vector2> points = PoissonDiskSampler.Generate(
                playableObjectSpacing,
                environmentObjectSpacing,
                mapTotalSize,
                playableMapSize,
                30,
                wall
            );

            float noiseScale;
            List<ProceduralObject> objects = new List<ProceduralObject>();

            foreach (Vector2 p in points)
            {
                float x = p.x - mapTotalSize.x / 2f;
                float z = p.y - mapTotalSize.y / 2f;

                noiseScale = playableNoiseScale;
                objects = playableObjects;

                Vector3 area = GetSafePlayableArea(playableMapSize,wall);
                
                if (!IsPointInside(Vector3.zero, area, new Vector3(x, 0, z))) 
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

                GameObject inst = Instantiate(obj.proceduralObject, pos, Quaternion.identity, _propsParent);

                // variación de escala
                float scale = Random.Range(obj.scaleRange.x, obj.scaleRange.y);
                inst.transform.localScale *= scale;

                _spawned.Add(inst);

            }
        }

        private void FindPlayerSpawnPosition(Vector2 playableArea, GameObject wall)
        {
            Vector3 safePlayableAreaVector3 = GetSafePlayableArea(playableArea, wall);
            Vector2 safePlayableArea = new Vector2(safePlayableAreaVector3.x,safePlayableAreaVector3.z);
            bool isPlayerGenerated = false;
            while (!isPlayerGenerated)
            {
                bool isSpawnPositionValid = false;
                float positionX = Random.Range(-safePlayableArea.x / 2, safePlayableArea.x / 2);
                float positionY = Random.Range(-safePlayableArea.y / 2, safePlayableArea.y / 2);
                Vector2 position = new Vector2(positionX, positionY);

                foreach (GameObject spawnedObject in _spawned)
                {
                    if (Vector2.Distance(position, spawnedObject.transform.position) > _safeSpawnDistance)
                    {
                        isSpawnPositionValid = true;
                    }
                    else
                    {
                        isSpawnPositionValid = false;
                        break;
                    }
                }

                if (isSpawnPositionValid)
                {
                    GeneratePlayer(position);
                    isPlayerGenerated = true;
                }
                
            }
        }

        private void GeneratePlayer(Vector2 spawnPosition)
        {
            _playerSpawnPosition =  spawnPosition;
            Instantiate(_playerPrefab, new Vector3(spawnPosition.x, 7, spawnPosition.y), Quaternion.identity, transform );
        }

        private void FindEnemiesSpawnPosition(Vector2 playableArea, GameObject wall, GameObject[] enemies, int numberOfEnemies)
        {
            Vector3 safePlayableAreaVector3 = GetSafePlayableArea(playableArea, wall);
            Vector2 safePlayableArea = new Vector2(safePlayableAreaVector3.x,safePlayableAreaVector3.z);
            List<Vector2> spawnPositions = new List<Vector2>();
            while (spawnPositions.Count < numberOfEnemies)
            {
                bool isSpawnPositionValid = false;
                float positionX = Random.Range(-safePlayableArea.x / 2, safePlayableArea.x / 2);
                float positionY = Random.Range(-safePlayableArea.y / 2, safePlayableArea.y / 2);
                Vector2 position = new Vector2(positionX, positionY);

                foreach (GameObject spawnedObject in _spawned) 
                {
                    if (Vector2.Distance(position, spawnedObject.transform.position) > _safeSpawnDistance && Vector2.Distance(position, _playerSpawnPosition) > _playerSafeSpawnEnemiesDistance)
                    {
                        isSpawnPositionValid = true;
                    }
                    else
                    {
                        isSpawnPositionValid = false;
                        break;
                    }
                }

                if (isSpawnPositionValid)
                {
                    spawnPositions.Add(position);
                }
                
            }
            
            GenerateEnemies(spawnPositions, enemies);
        }

        private void GenerateEnemies(List<Vector2> spawnPositions, GameObject[] enemies)
        {
            foreach (Vector2 spawnPosition in spawnPositions)
            {
                Instantiate(enemies[0],  new Vector3(spawnPosition.x, 5, spawnPosition.y), Quaternion.identity, _enemiesParent);
            }
        }
        
        private static class PoissonDiskSampler
        {
            public static List<Vector2> Generate(float playableRadius, float environmentRadius, Vector2 regionSize, Vector2 playableSize, int rejectionSamples, GameObject wall)
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
                        
                        bool isParentPointInsidePlayableArea = true;
                        cellSize = playableCellSize;
                        radius = playableRadius;
                        grid = playableGrid;

                        Vector3 playableArea = GetSafePlayableArea(playableSize,wall);
                    
                        if (!IsPointInside(new Vector3(regionSize.x /2, 0, regionSize.y /2), playableArea,new Vector3(spawnPoints[spawnIndex].x, 0, spawnPoints[spawnIndex].y) ))
                        {
                            cellSize = environmentCellSize;
                            radius = environmentRadius;
                            grid = environmentGrid;
                            isParentPointInsidePlayableArea = false;
                        }
                        
                        float angle = Random.value * Mathf.PI * 2;
                        float dist = Random.Range(radius, radius * 2);
                        Vector2 candidate = spawnCenter + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist;

                        if (IsPointInside(new Vector3(regionSize.x / 2, 0, regionSize.y / 2), playableArea,
                                new Vector3(candidate.x, 0, candidate.y))  && !isParentPointInsidePlayableArea)
                        {
                            continue;
                        }
                        
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