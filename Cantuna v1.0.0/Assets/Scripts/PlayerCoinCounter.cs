using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerCoinCounter : MonoBehaviour
{
    [Header("UI del Contador de Monedas")]
    [Tooltip("Texto de la interfaz para mostrar el contador de monedas.")]
    public TextMeshProUGUI coinCounterText;


    private int coinCount = 0;

    /// <summary>
    /// Incrementa el contador de monedas y actualiza la UI.
    /// </summary>
    /// <param name="amount">Cantidad de monedas a añadir.</param>
    public void AddCoins(int amount)
    {
        coinCount += amount;
        UpdateCoinCounterUI();
    }

    /// <summary>
    /// Actualiza la interfaz del contador de monedas.
    /// </summary>
    private void UpdateCoinCounterUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = "Piedras: " + coinCount;
        }
    }
}
