using UnityEngine;
using System.Collections.Generic;

public class InfiniteScenarioGenerator : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    [Header("Prefabs a instanciar en cada segmento")]
    public GameObject[] prefabsIzquierda;
    public GameObject[] prefabsDerecha;
    public GameObject pisoPrefab;

    [Header("Parámetros de posición")]
    public float zIzquierda = 113.12f;
    public float zDerecha = 97.29f;
    public float yAltura = 0.5f;
    public float pisoY = 0f;

    [Header("Offset y distancias")]
    public float distanciaEntreSegmentos = 20f;
    public float offsetEntrePrefabs = 2f;
    public int numSegmentosIniciales = 3;
    public float zonaSegura = 20f;

    private float spawnPosX;
    private List<GameObject> objetosGenerados = new List<GameObject>();

    void Start()
    {
        spawnPosX = -201.7f;
        for (int i = 0; i < numSegmentosIniciales; i++)
        {
            SpawnSegmento();
        }
    }

    void Update()
    {
        if (player.position.x + zonaSegura > spawnPosX)
        {
            SpawnSegmento();
        }
        RemoveOldSegments();
    }

    void SpawnSegmento()
    {
        if (pisoPrefab != null)
        {
            Vector3 posicionPiso = new Vector3(spawnPosX, pisoY, (zIzquierda + zDerecha) / 2);
            GameObject piso = Instantiate(pisoPrefab, posicionPiso, Quaternion.identity);
            objetosGenerados.Add(piso);
        }

        float offsetAcumulado = 0f;
        for (int i = 0; i < prefabsIzquierda.Length; i++)
        {
            if (prefabsIzquierda[i] == null) continue;
            Vector3 posicion = new Vector3(spawnPosX + offsetAcumulado, yAltura, zIzquierda);
            GameObject go = Instantiate(prefabsIzquierda[i], posicion, Quaternion.identity);
            objetosGenerados.Add(go);
            offsetAcumulado += offsetEntrePrefabs;
        }

        offsetAcumulado = 0f;
        for (int i = 0; i < prefabsDerecha.Length; i++)
        {
            if (prefabsDerecha[i] == null) continue;
            Vector3 posicion = new Vector3(spawnPosX + offsetAcumulado, yAltura, zDerecha);
            GameObject go = Instantiate(prefabsDerecha[i], posicion, Quaternion.identity);
            objetosGenerados.Add(go);
            offsetAcumulado += offsetEntrePrefabs;
        }

        spawnPosX += distanciaEntreSegmentos;
    }

    void RemoveOldSegments()
    {
        float limiteAtras = player.position.x - 50f;
        for (int i = objetosGenerados.Count - 1; i >= 0; i--)
        {
            if (objetosGenerados[i] == null)
            {
                objetosGenerados.RemoveAt(i);
                continue;
            }
            if (objetosGenerados[i].transform.position.x < limiteAtras)
            {
                Destroy(objetosGenerados[i]);
                objetosGenerados.RemoveAt(i);
            }
        }
    }
}
