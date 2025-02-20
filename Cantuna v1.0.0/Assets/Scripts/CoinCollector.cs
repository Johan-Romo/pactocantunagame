using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [Tooltip("Cantidad de puntos que esta moneda otorga al ser recogida.")]
    public int coinValue = 1;

    [Header("Audio")]
    [Tooltip("Clip de sonido que se reproducirá al recoger la moneda.")]
    public AudioClip coinSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerCoinCounter coinCounter = other.GetComponent<PlayerCoinCounter>();
            if (coinCounter != null)
            {
                coinCounter.AddCoins(coinValue);
            }
            EndlessRunnerPlayer player = other.GetComponent<EndlessRunnerPlayer>();
            if (player != null)
            {
                player.CollectCoin();
            }
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position, 1f);
            }
            Destroy(gameObject);
        }
    }
}