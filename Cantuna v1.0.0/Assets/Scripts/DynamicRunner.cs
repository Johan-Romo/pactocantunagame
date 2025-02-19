using UnityEngine;

public class DynamicRunner : MonoBehaviour
{
    [Header("Movimiento del Enemigo")]
    [Tooltip("Velocidad a la que el enemigo se desplaza en X (puede ser positiva o negativa según dirección).")]
    public float speed = 2f;

    // Opcional: si deseas que el enemigo se mueva en la misma dirección del jugador
    // (de X negativo a X positivo), usa speed positivo.
    // Si quieres lo opuesto, usa speed negativo.

    void Update()
    {
        // Mover en el eje X (pos. o neg.):
        // Vector3.right  => avanza en X positivo
        // Vector3.left   => avanza en X negativo

        // Ajusta según tu dirección deseada
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Opcional: si quieres que "camine" en Z en vez de X, usa Vector3.forward/back
        // transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
