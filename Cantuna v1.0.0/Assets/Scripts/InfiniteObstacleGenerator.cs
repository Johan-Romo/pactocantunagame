using UnityEngine;
using System.Collections.Generic;

public class InfiniteObstacle : MonoBehaviour
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
    public float minSpawnDistance = 10f;
    public float maxSpawnDistance = 15f;
    public int initialSegmentCount = 5;
    public float safeZoneDistance = 20f;
    public int coinsPerGroup = 5;
    public float coinSpacing = 1.5f;

    private float nextSpawnX;
    private List<GameObject> spawnedObjects = new List<GameObject>();

    //  IA: Contadores de rendimiento
    private int collisions = 0;
    private int coinsCollected = 0;
    private float survivalTime = 0f;

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
        survivalTime += Time.deltaTime; // Contar tiempo jugado

        if (player.position.x + safeZoneDistance > nextSpawnX)
        {
            SpawnSegment();
        }

        AdjustDifficulty(); // Ajustar dificultad según rendimiento
        RemoveOldObjects();
    }

    /// <summary>
    /// Ajusta la dificultad en función del rendimiento del jugador.
    /// </summary>
    void AdjustDifficulty()
    {
        // Si el jugador sobrevive más tiempo, aumentamos obstáculos
        if (survivalTime > 30)
        {
            minSpawnDistance = 8f;
            maxSpawnDistance = 12f;
        }
        else
        {
            minSpawnDistance = 10f;
            maxSpawnDistance = 15f;
        }

        // Si el jugador choca mucho, reducimos obstáculos
        if (collisions >= 3)
        {
            minSpawnDistance = 12f;
            maxSpawnDistance = 18f;
            collisions = 0; // Resetear contador
        }

        // Si recoge muchas monedas, reducimos cantidad generada
        if (coinsCollected > 10)
        {
            coinsPerGroup = Mathf.Max(1, coinsPerGroup - 1);
            coinsCollected = 0; // Resetear contador
        }
    }

    /// <summary>
    /// Registra colisiones del jugador.
    /// </summary>
    public void PlayerCollided()
    {
        collisions++;
    }

    /// <summary>
    /// Registra monedas recogidas.
    /// </summary>
    public void CoinCollected()
    {
        coinsCollected++;
    }

    /// <summary>
    /// Genera un segmento con obstáculos y monedas.
    /// </summary>
    void SpawnSegment()
    {
        List<int> blockedLanes = new List<int>();
        int lanesToBlock = Random.Range(1, 3);

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

        nextSpawnX += Random.Range(minSpawnDistance, maxSpawnDistance);
    }

    /// <summary>
    /// Genera un obstáculo en el carril especificado.
    /// </summary>
    void SpawnObstacle(float laneZ)
    {
        GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        Vector3 spawnPosition = new Vector3(nextSpawnX, obstacleHeight, laneZ);
        GameObject newObstacle = Instantiate(obstaclePrefab, spawnPosition, Quaternion.identity);
        spawnedObjects.Add(newObstacle);
    }

    /// <summary>
    /// Genera un grupo de monedas en el carril especificado.
    /// </summary>
    void SpawnCoinGroup(float laneZ)
    {
        for (int i = 0; i < coinsPerGroup; i++)
        {
            float offsetX = i * coinSpacing;
            Vector3 spawnPosition = new Vector3(nextSpawnX + offsetX, coinHeight, laneZ);
            GameObject newCoin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
            spawnedObjects.Add(newCoin);
        }
    }

    /// <summary>
    /// Elimina objetos que están muy atrás del jugador.
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
