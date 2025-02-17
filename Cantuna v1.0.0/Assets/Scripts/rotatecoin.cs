using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    [Header("Velocidad de Rotación")]
    [Tooltip("Velocidad de rotación de la moneda en grados por segundo.")]
    public float rotationSpeed = 100f;

    void Update()
    {
        // Aplica una rotación continua en el eje Y
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
