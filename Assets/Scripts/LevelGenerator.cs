using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public GameObject roadSegmentPrefab;
    public Transform player;
    public int initialSegments = 3;
    public float spawnDistanceThreshold = 75.0f;

    public List<GameObject> obstaclePrefabs;
    [Range(0, 1)]
    public float minObstacleSpawnChance = 0.1f;
    [Range(0, 1)]
    public float maxObstacleSpawnChance = 0.5f;
    public float timeToMaxDifficulty = 120.0f;

    public GameObject maskPrefab;
    [Range(0, 1)]
    public float maskSpawnChance = 0.2f;

    private List<GameObject> activeSegments = new List<GameObject>();
    private Transform nextSpawnPoint;

    void Start()
    {
        GameObject initialSegment = Instantiate(roadSegmentPrefab);
        initialSegment.transform.position = Vector3.zero;
        activeSegments.Add(initialSegment);
        nextSpawnPoint = initialSegment.transform.Find("SpawnPoint");

        for (int i = 1; i <= initialSegments; i++)
        {
            SpawnSegment(true);
        }

    }
    private void Update()
    {
        if (Vector3.Distance(player.position, nextSpawnPoint.position) < spawnDistanceThreshold)
        {
            SpawnSegment(true);
        }

        if(activeSegments.Count > initialSegments + 2)
        {
            GameObject segmentToDestroy = activeSegments[0];

            activeSegments.RemoveAt(0);

            Destroy(segmentToDestroy);
        }

    }

    void SpawnSegment(bool spawnItems)
    {
        GameObject newSegment = Instantiate(roadSegmentPrefab,nextSpawnPoint.position, nextSpawnPoint.rotation);
        activeSegments.Add(newSegment);

        nextSpawnPoint = newSegment.transform.Find("SpawnPoint");

        if (spawnItems && (obstaclePrefabs.Count > 0 || maskPrefab != null))
        {
            Transform spawnerParent = newSegment.transform.Find("ObstacleSpawners");

            if(spawnerParent != null && spawnerParent.childCount > 0    )
            {
                float progress = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);

                float currentObstacleChance = Mathf.Lerp(minObstacleSpawnChance, maxObstacleSpawnChance, progress);

                int laneCount = spawnerParent.childCount;

                int safeLaneIndex = Random.Range(0,laneCount);

                for (int i = 0; i < laneCount; i++)
                {
                    if(i == safeLaneIndex)
                    {
                        continue;
                    }

                    Transform spawner = spawnerParent.GetChild(i);

                    if(Random.value < currentObstacleChance)
                    {
                        int prefabIndex = Random.Range(0, obstaclePrefabs.Count);
                        GameObject obstacleToSpawn = obstaclePrefabs[prefabIndex];
                        Instantiate(obstacleToSpawn, spawner.position, spawner.rotation, spawner);

                    }
                    else
                    {
                        if(maskPrefab != null && Random.value < maskSpawnChance)
                        {
                            Instantiate(maskPrefab, spawner.position, spawner.rotation, spawner);
                        }
                    }
                }
            }
        }
    }
}
