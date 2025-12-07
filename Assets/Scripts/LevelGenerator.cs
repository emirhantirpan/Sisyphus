using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    [Header("Road Settings")]
    [SerializeField] private GameObject roadSegmentPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private int initialSegments = 3;
    [SerializeField] private float spawnDistanceThreshold = 75.0f;

    [Header("Obstacle Settings")]
    [SerializeField] private List<GameObject> obstaclePrefabs;
    [Range(0, 1)]
    [SerializeField] private float minObstacleSpawnChance = 0.1f;
    [Range(0, 1)]
    [SerializeField] private float maxObstacleSpawnChance = 0.5f;
    [SerializeField] private float timeToMaxDifficulty = 120.0f;

    [Header("Mask Settings")]
    [SerializeField] private GameObject maskPrefab;
    [Range(0, 1)]
    [SerializeField] private float maskSpawnChance = 0.2f;

    [Header("Coin Settings")]
    [SerializeField] private GameObject coinPrefab;
    [Range(0, 1)]
    [SerializeField] private float coinSpawnChance = 0.5f;

    private List<GameObject> activeSegments = new List<GameObject>();
    private Transform nextSpawnPoint;
    private int maxActiveSegments;

    private void Start()
    {
        maxActiveSegments = initialSegments + 2;
        InitializeLevel();
    }

    private void Update()
    {
        CheckAndSpawnSegment();
        CleanupOldSegments();
    }

    private void InitializeLevel()
    {
        SpawnInitialSegment();

        for (int i = 1; i <= initialSegments; i++)
        {
            SpawnSegment(true);
        }
    }

    private void SpawnInitialSegment()
    {
        GameObject initialSegment = Instantiate(roadSegmentPrefab);
        initialSegment.transform.position = Vector3.zero;
        activeSegments.Add(initialSegment);
        nextSpawnPoint = initialSegment.transform.Find("SpawnPoint");
    }

    private void CheckAndSpawnSegment()
    {
        if (player == null || nextSpawnPoint == null) return;

        float distanceToSpawnPoint = Vector3.Distance(player.position, nextSpawnPoint.position);

        if (distanceToSpawnPoint < spawnDistanceThreshold)
        {
            SpawnSegment(true);
        }
    }

    private void CleanupOldSegments()
    {
        if (activeSegments.Count > maxActiveSegments)
        {
            GameObject segmentToDestroy = activeSegments[0];
            activeSegments.RemoveAt(0);
            Destroy(segmentToDestroy);
        }
    }

    private void SpawnSegment(bool spawnItems)
    {
        GameObject newSegment = Instantiate(roadSegmentPrefab, nextSpawnPoint.position, nextSpawnPoint.rotation);
        activeSegments.Add(newSegment);
        nextSpawnPoint = newSegment.transform.Find("SpawnPoint");

        if (spawnItems)
        {
            SpawnItemsOnSegment(newSegment);
        }
    }

    private void SpawnItemsOnSegment(GameObject segment)
    {
        Transform spawnerParent = segment.transform.Find("ObstacleSpawners");

        if (spawnerParent == null || spawnerParent.childCount == 0) return;

        float currentObstacleChance = CalculateObstacleChance();
        int safeLaneIndex = Random.Range(0, spawnerParent.childCount);

        for (int i = 0; i < spawnerParent.childCount; i++)
        {
            Transform spawner = spawnerParent.GetChild(i);

            if (i == safeLaneIndex)
            {
                TrySpawnCoin(spawner);
            }
            else
            {
                SpawnLaneContent(spawner, currentObstacleChance);
            }
        }
    }

    private float CalculateObstacleChance()
    {
        float progress = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);
        return Mathf.Lerp(minObstacleSpawnChance, maxObstacleSpawnChance, progress);
    }

    private void SpawnLaneContent(Transform spawner, float obstacleChance)
    {
        if (Random.value < obstacleChance)
        {
            SpawnObstacle(spawner);
        }
        else if (maskPrefab != null && Random.value < maskSpawnChance)
        {
            SpawnMask(spawner);
        }
        else
        {
            TrySpawnCoin(spawner);
        }
    }

    private void SpawnObstacle(Transform spawner)
    {
        if (obstaclePrefabs == null || obstaclePrefabs.Count == 0) return;

        int randomIndex = Random.Range(0, obstaclePrefabs.Count);
        GameObject obstacleToSpawn = obstaclePrefabs[randomIndex];
        Instantiate(obstacleToSpawn, spawner.position, spawner.rotation, spawner);
    }

    private void SpawnMask(Transform spawner)
    {
        Instantiate(maskPrefab, spawner.position, spawner.rotation, spawner);
    }

    private void TrySpawnCoin(Transform spawner)
    {
        if (coinPrefab != null && Random.value < coinSpawnChance)
        {
            Instantiate(coinPrefab, spawner.position, spawner.rotation, spawner);
        }
    }
}