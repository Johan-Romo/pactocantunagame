using UnityEngine;
public class CoinCollector : MonoBehaviour
{
    [Tooltip("Cantidad de puntos que esta moneda otorga al ser recogida.")]
    public int coinValue = 1;

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que colisiona es el jugador
        if (other.CompareTag("Player"))
        {
            // Incrementa el contador de monedas en el jugador
            PlayerCoinCounter playerCounter = other.GetComponent<PlayerCoinCounter>();
            if (playerCounter != null)
            {
                playerCounter.AddCoins(coinValue);
            }

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}
