using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
public class UIController : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelLoby;
    public GameObject panelGame;
    public GameObject panelGameOver;
    public GameObject panelWin;

    [Header("Lobby UI")]
    public TMP_InputField inputName;
    public TMP_Text lobbyMsg;
    public TMP_Text playerList;
    public Button addButton;
    public Button startButton;
    public Button quitButton;

    private void Start()
    {
        ShowLobby();
        RefreshPlayerList();
        QuitPlayer();
        lobbyMsg.text = "";
    }
    private void Update()
    {
        if (GameManager.Instance.IsPlaying == true)
        {
            UpdateGameUI();
        }
    }

    public void OnAddPlayer()
    {
        string name = inputName.text;
        bool ok = GameManager.Instance.AddPlayer(name);
        if (ok == true)
        {
            inputName.text = "";
            RefreshPlayerList();
            lobbyMsg.text = "";
        }
        else
        {
            lobbyMsg.text = "Nombre invalido o ya hay 4 jugadores";
        }
    }

    public void ShowLobby()
    {
        panelLoby.SetActive(true);
    }

    public void RefreshPlayerList()
    {
        
    }
    public void QuitPlayer()
    {
       
    }
    public void UpdateGameUI()
    {
        panelLoby.SetActive(false);
        panelGame.SetActive(true);
    }
}
