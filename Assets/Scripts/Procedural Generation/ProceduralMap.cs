using System;
using System.Collections.Generic;
using Player;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralGeneration
{
    public class ProceduralMap : MonoBehaviour
    {

        [Header("NavMesh")] public NavMeshSurface navSurface;

        [Header("Player")] public GameObject playerPrefab;

        private GameObject terrainPlane;
        private List<GameObject> spawned = new List<GameObject>();

        private List<Transform> patrolPoints = new List<Transform>();
        private List<Transform> spawnedEnemiesPosition = new List<Transform>();

        private Transform propsParent;
        private Transform enemiesParent;
        private Transform patrolPointsParent;

        public void ClearMap()
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }

        public void Generate(int seed, Biome biome, Vector2 mapSize,
            float objectSpacing, float noiseScale, GameObject[] enemyPrefabs, float enemyMinDistance,
            int enemyCount, GameObject patrolPointPrefab, float patrolPointsMinDistance, int patrolPointsCount,
            bool generatePlayer)
        {
            ClearMap();
            GeneratePlane(mapSize, biome);
            GenerateParents();
            GenerateProps(seed, biome, objectSpacing, mapSize, noiseScale);
            BakeNavMesh();
            if (generatePlayer)
            {
                SpawnPlayer(mapSize);
            }

            SpawnEnemies(enemyMinDistance, mapSize, enemyCount, enemyPrefabs);
            SpawnPatrolPoints(patrolPointsMinDistance, mapSize, patrolPointsCount, patrolPointPrefab);
        }

        private void GeneratePlane(Vector2 mapSize, Biome biome)
        {
            terrainPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            terrainPlane.transform.SetParent(transform);
            terrainPlane.transform.localPosition = Vector3.zero;
            terrainPlane.transform.localScale = new Vector3(mapSize.x / 10f, 1f, mapSize.y / 10f);

            terrainPlane.GetComponent<Renderer>().material = biome.terrainMaterial;
        }

        public void GenerateParents()
        {
            propsParent = new GameObject("Props").transform;
            propsParent.SetParent(transform);

            enemiesParent = new GameObject("Enemies").transform;
            enemiesParent.SetParent(transform);

            patrolPointsParent = new GameObject("Patrol points").transform;
            patrolPointsParent.SetParent(transform);
        }

        public void GenerateProps(int seed, Biome biome, float objectSpacing, Vector2 mapSize, float noiseScale)
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
                ProceduralObject obj = ObjectRuleEngine.SelectObject(density, biome.proceduralObjects);

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

        private void BakeNavMesh()
        {
            if (navSurface != null)
                navSurface.BuildNavMesh();
        }

        private void SpawnPlayer(Vector2 mapSize) //TODO: Spawn far from enemies
        {
            if (playerPrefab == null) return;

            Vector3 center = new Vector3(0, 5f, 0);

            UnityEngine.AI.NavMeshHit hit;

            if (UnityEngine.AI.NavMesh.SamplePosition(center, out hit, 20f, UnityEngine.AI.NavMesh.AllAreas))
            {
                Instantiate(playerPrefab, hit.position + Vector3.up + new Vector3(0, 3f, 0), Quaternion.identity,
                    transform);
            }

        }

        private void SpawnEnemies(float enemyMinDistance, Vector2 mapSize, int enemyCount, GameObject[] enemies)
        {
            if (enemies == null) return;
            spawnedEnemiesPosition.Clear();
            // Creamos posiciones Poisson para enemigos
            List<Vector2> points = PoissonDiskSampler.Generate(
                enemyMinDistance,
                mapSize,
                20
            );

            // Limitamos a enemyCount
            for (int i = 0; i < Mathf.Min(enemyCount, points.Count); i++)
            {
                Vector2 p = points[i];
                float x = p.x - mapSize.x / 2f;
                float z = p.y - mapSize.y / 2f;

                Vector3 pos = new Vector3(x, 3f, z);

                GameObject spawnedEnemy = Instantiate(GetRandomEnemy(enemies), pos, Quaternion.identity, enemiesParent);
                spawnedEnemiesPosition.Add(spawnedEnemy.transform);
            }
        }

        private GameObject GetRandomEnemy(GameObject[] enemies)
        {
            return enemies[Random.Range(0, enemies.Length)];
        }

        private void SpawnPatrolPoints(float patrolPointsMinDistance, Vector2 mapSize, int patrolPointsCount,
            GameObject patrolPointPrefab)
        {
            patrolPoints.Clear();

            List<Vector2> points = PoissonDiskSampler.Generate(
                patrolPointsMinDistance,
                mapSize,
                20
            );

            for (int i = 0; i < Mathf.Min(points.Count, patrolPointsCount); i++)
            {
                Vector2 p = points[i];
                float x = p.x - mapSize.x / 2f;
                float z = p.y - mapSize.y / 2f;

                Vector3 pos = new Vector3(x, 0f, z);

                UnityEngine.AI.NavMeshHit hit;

                if (UnityEngine.AI.NavMesh.SamplePosition(pos, out hit, 5f, UnityEngine.AI.NavMesh.AllAreas))
                {
                    GameObject point = Instantiate(patrolPointPrefab, pos, Quaternion.identity, patrolPointsParent);
                    patrolPoints.Add(point.transform);
                }
            }

        }

        public Transform[] getPatrolPoints()
        {
            return patrolPoints.ToArray();
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
