using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreenUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject endScreenPanel;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text winnerInfoText;
    [SerializeField] private Button restartButton;

    private void Start()
    {
        // El panel debe estar oculto por defecto.
        endScreenPanel.SetActive(false);

        // Suscribirse al evento del botón de reinicio.
        restartButton.onClick.AddListener(OnRestartClicked);
    }

    private void OnDestroy()
    {
        // Darse de baja del evento.
        restartButton.onClick.RemoveListener(OnRestartClicked);
    }

    public void Show(PlayerData winner)
    {
        if (winner != null)
        {
            winnerInfoText.text = $"El ganador es {winner.playerName}\n¡Con un puntaje de {winner.score}!";
        }
        else
        {
            // En caso de un empate o error, muestra un mensaje genérico.
            winnerInfoText.text = "¡El juego ha terminado!";
        }
        
        endScreenPanel.SetActive(true);
    }

    private void OnRestartClicked()
    {
        // La forma más robusta de reiniciar un juego es recargar la escena.
        // Esto resetea todos los estados y scripts a su valor inicial.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}