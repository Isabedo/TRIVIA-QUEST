using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestionController : MonoBehaviour
{
    public static QuestionController Instance { get; private set; }

    [SerializeField] private List<Question> allQuestions;
    
    private List<Question> remainingQuestions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PrepareNewGame()
    {
        if (allQuestions == null || allQuestions.Count == 0)
        {
            Debug.LogError("QuestionController: No questions have been assigned in the Inspector.");
            return;
        }

        // Creamos una copia aleatoria de la lista de preguntas para la partida actual.
        remainingQuestions = allQuestions.OrderBy(q => System.Guid.NewGuid()).ToList();
    }

    public Question GetNextQuestion()
    {
        if (remainingQuestions == null || remainingQuestions.Count == 0)
        {
            PrepareNewGame();
        }

        Question nextQuestion = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);

        return nextQuestion;
    }
}