using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TurnController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int totalRounds = 10;
    
    [Header("Component References")]
    [SerializeField] private GameplayUI gameplayUI;

    private List<PlayerData> players;
    private Question currentQuestion;
    
    private int currentPlayerIndex = -1;
    private int currentRound = 1;
    
    private void OnEnable()
    {
        // Suscribirse al evento de respuesta seleccionada.
        GameplayUI.OnAnswerSelected += HandleAnswerSelected;
    }

    private void OnDisable()
    {
        // Darse de baja para evitar errores.
        GameplayUI.OnAnswerSelected -= HandleAnswerSelected;
    }

    public void StartGame(List<PlayerData> playerList)
    {
        if (playerList == null || playerList.Count == 0)
        {
            Debug.LogError("TurnController: Cannot start game with no players.");
            return;
        }

        players = playerList;
        currentRound = 1;
        currentPlayerIndex = -1; // Se incrementará a 0 en el primer llamado.

        AdvanceTurn();
    }

    private void HandleAnswerSelected(int selectedIndex)
    {
        bool isCorrect = (selectedIndex == currentQuestion.correctAnswerIndex);
        if (isCorrect)
        {
            players[currentPlayerIndex].score++;
            Debug.Log($"{players[currentPlayerIndex].playerName} answered correctly! New score: {players[currentPlayerIndex].score}");
        }
        else
        {
            Debug.Log($"{players[currentPlayerIndex].playerName} answered incorrectly.");
        }

        // Una vez procesada la respuesta, pasamos al siguiente turno.
        AdvanceTurn();
    }

    private void AdvanceTurn()
    {
        currentPlayerIndex++;

        // Si hemos completado una vuelta de jugadores...
        if (currentPlayerIndex >= players.Count)
        {
            currentRound++;
            currentPlayerIndex = 0;
        }

        // Si hemos completado todas las rondas...
        if (currentRound > totalRounds)
        {
            EndGame();
            return;
        }

        // Si el juego continúa, obtenemos una pregunta y se la pasamos a la UI.
        currentQuestion = QuestionController.Instance.GetNextQuestion();
        gameplayUI.DisplayQuestion(currentQuestion, players[currentPlayerIndex].playerName);
    }

    private void EndGame()
    {
        Debug.Log("Game Over! Calculating scores...");
        
        // Encontrar al jugador con la puntuación más alta.
        PlayerData winner = players.OrderByDescending(p => p.score).FirstOrDefault();

        if (winner != null)
        {
            Debug.Log($"The winner is {winner.playerName} with a score of {winner.score}!");
            // Aquí, en el futuro, guardaremos los datos del ganador para la pantalla final.
        }

        // Notificar al GameManager que el juego ha terminado.
        GameManager.Instance.EndGame(winner);
    }
}