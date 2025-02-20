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

    [Header("Parámetros de posición")]
    public float zIzquierda = 113.12f;
    public float zDerecha = 97.29f;
    public float yAltura = 0.5f;
    public float pisoY = 0f;

    [Header("Offset y distancias")]
    // Distancia en X que avanza cada "segmento"
    public float distanciaEntreSegmentos = 20f;

    // Distancia FIJA que dejamos entre cada prefab del lado izquierdo
    public float espacioEntreCasasIzq = 15f;

    // Distancia FIJA que dejamos entre cada prefab del lado derecho
    public float espacioEntreCasasDer = 15f;

    // Cantidad de segmentos que se generan al inicio
    public int numSegmentosIniciales = 3;

    // Distancia con la que adelantamos la generación de un nuevo segmento
    public float zonaSegura = 20f;

    // Posición en X donde colocaremos el próximo segmento
    private float spawnPosX;

    private List<GameObject> objetosGenerados = new List<GameObject>();

    void Start()
    {
        // Punto inicial en X
        spawnPosX = -201.7f;

        // Generamos unos cuantos segmentos de arranque
        for (int i = 0; i < numSegmentosIniciales; i++)
        {
            SpawnSegmento();
        }
    }

    void Update()
    {
        // Si el jugador se acerca al final del último segmento, generamos otro
        if (player.position.x + zonaSegura > spawnPosX)
        {
            SpawnSegmento();
        }

        // Limpiamos objetos muy atrás para no sobrecargar la escena
        RemoveOldSegments();
    }

    void SpawnSegmento()
    {
        // 1) Generar el piso (opcional)
        if (pisoPrefab != null)
        {
            Vector3 posicionPiso = new Vector3(
                spawnPosX,
                pisoY,
                (zIzquierda + zDerecha) / 2f
            );

            GameObject piso = Instantiate(pisoPrefab, posicionPiso, Quaternion.identity);
            objetosGenerados.Add(piso);
        }

        // 2) Generar los prefabs del lado IZQUIERDO
        float currentXPosIzq = spawnPosX;
        for (int i = 0; i < prefabsIzquierda.Length; i++)
        {
            var prefab = prefabsIzquierda[i];
            if (prefab == null) continue;

            Vector3 posicion = new Vector3(
                currentXPosIzq,
                yAltura,
                zIzquierda
            );

            GameObject go = Instantiate(prefab, posicion, Quaternion.identity);
            objetosGenerados.Add(go);

            // Avanzamos para el siguiente edificio
            currentXPosIzq += espacioEntreCasasIzq;
        }

        // 3) Generar los prefabs del lado DERECHO
        float currentXPosDer = spawnPosX;
        for (int i = 0; i < prefabsDerecha.Length; i++)
        {
            var prefab = prefabsDerecha[i];
            if (prefab == null) continue;

            Vector3 posicion = new Vector3(
                currentXPosDer,
                yAltura,
                zDerecha
            );

            GameObject go = Instantiate(prefab, posicion, Quaternion.identity);
            objetosGenerados.Add(go);

            // Avanzamos para el siguiente edificio
            currentXPosDer += espacioEntreCasasDer;
        }

        // 4) Calculamos la posición más alejada para el siguiente segmento
        float maxXIzquierda = spawnPosX + (prefabsIzquierda.Length * espacioEntreCasasIzq);
        float maxXDerecha = spawnPosX + (prefabsDerecha.Length * espacioEntreCasasDer);
        float maxX = Mathf.Max(maxXIzquierda, maxXDerecha);

        // Aseguramos un espacio mínimo entre segmentos
        spawnPosX = maxX + distanciaEntreSegmentos;
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