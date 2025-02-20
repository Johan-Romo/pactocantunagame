using UnityEngine;

public class ChurchHorizon : MonoBehaviour
{
    [Header("Jugador (o cámara) a seguir")]
    public Transform player;

    [Header("Offsets de posición")]
    // A qué distancia en X quieres que esté siempre delante del jugador
    public float distanceAhead = 300f;

    // Ajusta la altura y posición en Z para que se vea al fondo
    public float fixedY = 10f;
    public float fixedZ = 100f;

    void Update()
    {
        if (player == null) return;

        // La iglesia se coloca "distanceAhead" unidades delante del jugador en el eje X
        // y en la misma posición Y/Z fijas (o con otro offset que tú quieras)
        transform.position = new Vector3(
            player.position.x + distanceAhead,
            fixedY,
            fixedZ
        );
    }
}