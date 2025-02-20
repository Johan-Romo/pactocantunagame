using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("UI de Pausa")]
    public GameObject pausePanel;  // Panel de pausa

    private bool isPaused = false;

    private void Update()
    {
        // Presionar Enter para pausar/despausar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TogglePause();
        }
    }

    /// <summary>
    /// Alterna entre pausar y reanudar el juego.
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;  // Pausa el tiempo del juego
            pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;  // Reanuda el tiempo del juego
            pausePanel.SetActive(false);
        }
    }
}
