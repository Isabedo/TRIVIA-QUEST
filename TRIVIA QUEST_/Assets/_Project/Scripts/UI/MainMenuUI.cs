using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;

    [Header("Player Input")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private Button savePlayerButton;

    [Header("Player Display Grid")]
    [SerializeField] private Button[] playerSlotButtons; // Array para los 4 botones del grid

    [Header("Game Controls")]
    [SerializeField] private Button startGameButton;

    private List<PlayerData> tempPlayers = new List<PlayerData>();
    private TMP_Text[] playerSlotTexts; // Para acceder al texto de cada botón eficientemente

    private const int MinPlayers = 2;
    private const int MaxPlayers = 4;

    private void Start()
    {
        // Suscribirse a los eventos de los botones
        savePlayerButton.onClick.AddListener(OnSavePlayerClicked);
        startGameButton.onClick.AddListener(OnStartGameClicked);

        // Inicializar los componentes de texto y los listeners de los botones del grid
        playerSlotTexts = new TMP_Text[playerSlotButtons.Length];
        for (int i = 0; i < playerSlotButtons.Length; i++)
        {
            playerSlotTexts[i] = playerSlotButtons[i].GetComponentInChildren<TMP_Text>();

            int buttonIndex = i; // Captura de variable para el listener
            playerSlotButtons[i].onClick.AddListener(() => OnPlayerSlotClicked(buttonIndex));
        }

        InitializeMenu();
    }

    private void OnDestroy()
    {
        // Darse de baja de los eventos para evitar fugas de memoria
        savePlayerButton.onClick.RemoveListener(OnSavePlayerClicked);
        startGameButton.onClick.RemoveListener(OnStartGameClicked);
        for (int i = 0; i < playerSlotButtons.Length; i++)
        {
            playerSlotButtons[i].onClick.RemoveAllListeners();
        }
    }

    private void InitializeMenu()
    {
        tempPlayers.Clear();
        nameInputField.text = string.Empty;
        UpdatePlayerGrid();
        ValidateStartButton();
        ValidateInputInteractable();
        mainMenuPanel.SetActive(true);
    }

    private void OnSavePlayerClicked()
    {
        if (tempPlayers.Count >= MaxPlayers) return;

        string playerName = nameInputField.text;
        if (string.IsNullOrWhiteSpace(playerName)) return;

        tempPlayers.Add(new PlayerData(playerName));

        nameInputField.text = string.Empty;
        nameInputField.Select();
        nameInputField.ActivateInputField();

        UpdatePlayerGrid();
        ValidateStartButton();
        ValidateInputInteractable();
    }

    private void OnPlayerSlotClicked(int slotIndex)
    {
        if (slotIndex < tempPlayers.Count)
        {
            tempPlayers.RemoveAt(slotIndex);

            UpdatePlayerGrid();
            ValidateStartButton();
            ValidateInputInteractable();
        }
    }

    private void OnStartGameClicked()
    {
        GameManager.Instance.StartGame(tempPlayers);
        mainMenuPanel.SetActive(false);
    }

    private void UpdatePlayerGrid()
    {
        // Recorrer los 4 slots de botones
        for (int i = 0; i < playerSlotButtons.Length; i++)
        {
            // ¿Hay un jugador en la lista para este slot?
            if (i < tempPlayers.Count)
            {
                // Sí -> Muestra el botón y actualiza su texto
                playerSlotButtons[i].gameObject.SetActive(true);
                playerSlotTexts[i].text = tempPlayers[i].playerName;
            }
            else
            {
                // No -> Oculta el botón
                playerSlotButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void ValidateStartButton()
    {
        startGameButton.interactable = (tempPlayers.Count >= MinPlayers && tempPlayers.Count <= MaxPlayers);
    }
    
    private void ValidateInputInteractable()
    {
        bool isFull = tempPlayers.Count >= MaxPlayers;

        savePlayerButton.interactable = !isFull;
        nameInputField.interactable = !isFull;
    }
}