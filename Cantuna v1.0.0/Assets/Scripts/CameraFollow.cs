using UnityEngine;

/// <summary>
/// Script para que la cámara siga al jugador en un Endless Runner
/// cuyo avance principal es en el eje X.
/// 
/// Características:
/// - Mantiene un offset personalizado respecto al jugador.
/// - Usa Vector3.SmoothDamp para un seguimiento suave.
/// - Opción para mantener una rotación fija o mirar al jugador.
/// </summary>
public class EndlessRunnerCameraFollow : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    [Tooltip("El Transform del jugador que la cámara debe seguir.")]
    public Transform target;

    [Header("Offsets y Ajustes")]
    [Tooltip("Desplazamiento en posición relativo al jugador (X, Y, Z).")]
    public Vector3 offsetPosition = new Vector3(-10f, 5f, 0f);

    [Tooltip("Tiempo de suavizado (entre más bajo, menos suave).")]
    public float smoothTime = 0.2f;

    [Header("Rotación")]
    [Tooltip("¿Mantener una rotación fija en la cámara? Si no, la cámara mirará al jugador.")]
    public bool useFixedRotation = true;

    [Tooltip("Ángulos de la cámara si se usa rotación fija (en Euler angles).")]
    public Vector3 fixedRotationEuler = new Vector3(10f, 90f, 0f);

    // Almacena la velocidad interna usada por SmoothDamp
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null)
            return;

        // Posición deseada: La posición del jugador + el offset
        Vector3 desiredPosition = target.position + offsetPosition;

        // Movimiento suave entre la posición actual y la deseada
        Vector3 smoothedPosition = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );

        // Se asigna la nueva posición a la cámara
        transform.position = smoothedPosition;

        // Manejo de la rotación
        if (useFixedRotation)
        {
            // Mantener una rotación fija en todo momento
            transform.rotation = Quaternion.Euler(fixedRotationEuler);
        }
        else
        {
            // Si no quieres rotación fija, puedes hacer que la cámara mire al jugador:
            transform.LookAt(target);
        }
    }
}
