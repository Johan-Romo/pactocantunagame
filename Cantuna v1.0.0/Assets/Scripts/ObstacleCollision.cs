using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔥 Reduce la velocidad del jugador
            EndlessRunnerPlayer player = other.GetComponent<EndlessRunnerPlayer>();
            if (player != null)
            {
                player.SlowDown();  // ⬅ Se reduce la velocidad correctamente
            }

            // 🔥 Aplica daño al jugador si hay sistema de vidas
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage();
            }
        }
    }
}
