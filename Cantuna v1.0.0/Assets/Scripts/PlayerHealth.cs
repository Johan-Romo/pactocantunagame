using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PlayerHealth : MonoBehaviour
{
    [Header("Configuración de Vidas")]
    [Tooltip("Cantidad inicial de vidas del jugador.")]
    public int maxLives = 3;

    private int currentLives;

    [Header("UI de Vidas")]
    [Tooltip("Texto de la interfaz para mostrar las vidas restantes.")]
    public TextMeshProUGUI livesCounterText;
    [Header("Game Over")]
    [Tooltip("Panel de Game Over que se activa cuando las vidas llegan a 0.")]
    public GameObject gameOverPanel;
    public Image iconVida;
    public Image iconPiedra;



    private void Start()
    {
        // Inicializa las vidas
        currentLives = maxLives;

        // Actualiza la UI de vidas
        UpdateLivesUI();

        // Asegúrate de que el panel de Game Over esté desactivado al inicio
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Reduce las vidas del jugador.
    /// </summary>
    public void TakeDamage()
    {
        // Reduce las vidas
        currentLives--;

        // Actualiza la UI de vidas
        UpdateLivesUI();

        // Verifica si el jugador ha perdido todas las vidas
        if (currentLives <= 0)
        {
            GameOver();
        }
    }

    /// <summary>
    /// Actualiza la UI que muestra las vidas restantes.
    /// </summary>
    private void UpdateLivesUI()
    {
        if (livesCounterText != null)
        {
            livesCounterText.text = "Vidas: " + currentLives;
        }
    }

    /// <summary>
    /// Maneja el estado de Game Over.
    /// </summary>
    private void GameOver()
    {

        iconVida.gameObject.SetActive(false);
        iconPiedra.gameObject.SetActive(false);

        // Activa el panel de Game Over si está configurado
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // Desactiva el movimiento del jugador
        EndlessRunnerPlayer movementScript = GetComponent<EndlessRunnerPlayer>();
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }
    }
}
