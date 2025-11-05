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
    public float obstacleSpawnChance = 0.5f;

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

    void SpawnSegment(bool spawnObstacles)
    {
        GameObject newSegment = Instantiate(roadSegmentPrefab,nextSpawnPoint.position, nextSpawnPoint.rotation);
        activeSegments.Add(newSegment);

        nextSpawnPoint = newSegment.transform.Find("SpawnPoint");

        if (spawnObstacles && obstaclePrefabs.Count > 0)
        {
            Transform spawnerParent = newSegment.transform.Find("ObstacleSpawners");

            if(spawnerParent != null)
            {
                foreach (Transform spawner in spawnerParent)
                {
                    if(Random.value < obstacleSpawnChance)
                    {
                        int prefabIndex = Random.Range(0,obstaclePrefabs.Count);
                        GameObject obstacleToSpawn = obstaclePrefabs[prefabIndex];

                        Instantiate(obstacleToSpawn, spawner.position, spawner.rotation,spawner);
                    }
                }
            }
        }
    }
}
