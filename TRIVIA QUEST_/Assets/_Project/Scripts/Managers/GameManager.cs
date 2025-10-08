using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Component References")]
    [SerializeField] private TurnController turnController;
    [SerializeField] private EndScreenUI endScreenUI;

    public enum GameState
    {
        MainMenu,
        Gameplay,
        GameOver
    }

    public GameState CurrentState { get; private set; }

    // La lista de jugadores ahora es privada para cumplir con la encapsulación.
    // Otros scripts podrán leerla, pero solo el GameManager podrá modificarla.
    private List<PlayerData> players = new List<PlayerData>();
    
    // Propiedad pública de solo lectura para acceder a la lista de jugadores de forma segura.
    public IReadOnlyList<PlayerData> Players => players;
    
    private PlayerData gameWinner;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        // El juego siempre empieza en el menú principal.
        ChangeState(GameState.MainMenu);
    }

    public void StartGame(List<PlayerData> playerList)
    {
        // Se crea una nueva lista para evitar problemas de referencias con la lista temporal de la UI.
        players = new List<PlayerData>(playerList);
        gameWinner = null;
        ChangeState(GameState.Gameplay);
    }

    public void EndGame(PlayerData winner)
    {
        gameWinner = winner;
        ChangeState(GameState.GameOver);
    }

    private void ChangeState(GameState newState)
    {
        if (CurrentState == newState) return;

        CurrentState = newState;
        
        switch (CurrentState)
        {
            case GameState.MainMenu:
                Debug.Log("Game State: Main Menu");
                // Limpiamos la lista de jugadores por si se está reiniciando una partida.
                players.Clear();
                break;
                
            case GameState.Gameplay:
                Debug.Log("Game State: Gameplay Started!");
                // Prepara el banco de preguntas.
                QuestionController.Instance.PrepareNewGame();
                // Inicia el sistema de turnos.
                turnController.StartGame(players);
                break;
                
            case GameState.GameOver:
                Debug.Log("Game State: Game Over");
                endScreenUI.Show(gameWinner);
                break;
        }
    }
}