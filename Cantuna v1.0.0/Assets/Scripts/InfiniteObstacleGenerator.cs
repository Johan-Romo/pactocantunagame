using UnityEngine;
using System.Collections.Generic;

public class InfiniteObstacleAndCoinGenerator : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Prefabs")]
    public GameObject[] obstaclePrefabs;
    public GameObject coinPrefab;

    [Header("Parámetros de Generación")]
    public float[] lanePositionsZ = { -2f, 0f, 2f };
    public float obstacleHeight = 0.5f;
    public float coinHeight = 1.5f;
    public float minSpawnDistance = 8f;
    public float maxSpawnDistance = 15f;
    public int initialSegmentCount = 5;
    public float safeZoneDistance = 20f;
    public int coinsPerGroup = 5;
    public float coinSpacing = 1.5f;

    private float nextSpawnX;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    // 🔥 Variables de IA Adaptativa
    private float survivalTime = 0f;
    private float maxDifficulty = 7.0f;
    private float obstacleIncreaseRate = 0.2f;

    void Start()
    {
        nextSpawnX = player.position.x;
        for (int i = 0; i < initialSegmentCount; i++)
        {
            SpawnSegment();
        }
    }

    void Update()
    {
        survivalTime += Time.deltaTime;

        if (player.position.x + safeZoneDistance > nextSpawnX)
        {
            SpawnSegment();
        }

        RemoveOldObjects(); // 🔥 Se volvió a incluir esta función
    }

    void SpawnSegment()
    {
        List<int> blockedLanes = new List<int>();

        // 🔥 Aumentar la cantidad de obstáculos con el tiempo
        int lanesToBlock = Random.Range(1, Mathf.Clamp(1 + (int)(survivalTime * obstacleIncreaseRate), 1, 3));

        while (blockedLanes.Count < lanesToBlock)
        {
            int randomLane = Random.Range(0, lanePositionsZ.Length);
            if (!blockedLanes.Contains(randomLane))
            {
                blockedLanes.Add(randomLane);
                SpawnObstacle(lanePositionsZ[randomLane]);
            }
        }

        for (int i = 0; i < lanePositionsZ.Length; i++)
        {
            if (!blockedLanes.Contains(i))
            {
                SpawnCoinGroup(lanePositionsZ[i]);
            }
        }

        float difficultyFactor = Mathf.Clamp(1.0f + (survivalTime / 80f), 1.0f, maxDifficulty);
        float spawnDistance = Mathf.Lerp(maxSpawnDistance, minSpawnDistance, difficultyFactor / maxDifficulty);
        nextSpawnX += spawnDistance;
    }

    void SpawnObstacle(float laneZ)
    {
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPosition = new Vector3(nextSpawnX, obstacleHeight, laneZ);
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(newObstacle);
    }

    void SpawnCoinGroup(float laneZ)
    {
        int coinsToSpawn = Mathf.Max(1, coinsPerGroup - (int)(survivalTime / 20));

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float offsetX = i * coinSpacing;
            Vector3 spawnPosition = new Vector3(nextSpawnX + offsetX, coinHeight, laneZ);
            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newCoin);
        }
    }

    /// <summary>
    /// Elimina obstáculos y monedas que quedaron atrás del jugador.
    /// </summary>
    void RemoveOldObjects()
    {
        float removalThreshold = player.position.x - 50f;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--)
        {
            if (spawnedObjects[i] == null)
            {
                spawnedObjects.RemoveAt(i);
                continue;
            }

            if (spawnedObjects[i].transform.position.x < removalThreshold)
            {
                Destroy(spawnedObjects[i]);
                spawnedObjects.RemoveAt(i);
            }
        }
    }
}
