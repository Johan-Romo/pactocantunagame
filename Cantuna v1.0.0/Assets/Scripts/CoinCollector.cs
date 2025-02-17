using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [Tooltip("Cantidad de puntos que esta moneda otorga al ser recogida.")]
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 🔥 Obtener la referencia del contador de monedas
            PlayerCoinCounter coinCounter = other.GetComponent<PlayerCoinCounter>();
            if (coinCounter != null)
            {
                coinCounter.AddCoins(coinValue); // ⬅ Ahora se suma correctamente
            }

            // 🔥 Asegurar que la IA del jugador reciba el evento de recogida de moneda
            EndlessRunnerPlayer player = other.GetComponent<EndlessRunnerPlayer>();
            if (player != null)
            {
                player.CollectCoin();
            }

            // Destruir la moneda después de recogerla
            Destroy(gameObject);
        }
    }
}
