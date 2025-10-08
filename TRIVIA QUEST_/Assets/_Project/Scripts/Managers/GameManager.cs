using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //singleton

    public static GameManager Instance;

    //Revisar si ya esta creado
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [Header("Reglas")]

    public int pointsPerCompleted = 1;
    public float QuestionsTime = 10f;
    public Difficulty currentDifficulty = Difficulty.Easy;

    public bool IsPlaying = false;
    

    private List<PlayerData> players = new List<PlayerData>(); //lista de jugadores
    private int currentTurnIndex = 0;

    public Question CurrentQuestion = null;
    public float CurrentTime = 0f;

    private void Update()
    {
        if (IsPlaying == false)
        {
            return;
        }
        if (CurrentQuestion == null)
        {
            return;
        }
        if (CurrentTime > 0f)
        {
            CurrentTime -= Time.deltaTime;
            if (CurrentTime <= 0f)
            {
                YouLoose();
            }
        }
    }
    public bool AddPlayer(string name)
    {
        if (IsPlaying == true)
        {
            return false;
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }
        if (players.Count >= 4)
        {
            return false;
        }
        players.Add(new PlayerData(name.Trim()));
        return true;
    }
    public void ClearPlayer()
    {
        if (IsPlaying == true)
        {
            return;
        }
        players.Clear();

    }
    public bool CanStart()
    {
        if (players.Count < 2)
        {
            return false;
        }
        return true;
    }
    public void StartGame()
    {
        if (CanStart() == false)
        {
            return;
        }
        IsPlaying = true;
        currentTurnIndex = 0;

        //aqui iria el que va a seleccionar los retos de manera aleatoria
    }

    //Hacer por aca los codigos que permita avanzar de pregunta

    private void EndTurnAdvance()
    {
        if (players.Count < 2)
        {
            return;
        }
        currentTurnIndex += 1;
        if (currentTurnIndex >= players.Count)
        {
            currentTurnIndex = 0;
        }

        //metodo que cambia la pregunta
    }
    public void ApplyCompleted()
    {
        if (IsPlaying == false)
        {
            return;
        }
        if (players.Count == 0)
        {
            return;
        }
        PlayerData cur = players[currentTurnIndex];
        if (CurrentQuestion != null)
        {
            cur.score += pointsPerCompleted;
        }
        //metodo de avanzar la pregunta
    }
    public void YouLoose()
    {
        if (IsPlaying == false)
        {
            return;
        }
        //metodo de avanzar la pregunta
    }
    public int GetPlayerCount()
    {
        return players.Count;
    }

    public PlayerData GetPlayer(int index)
    {
        if (index < 0 || index >= players.Count)
        {
            return null;
        }
        return players[index];
    }
}
