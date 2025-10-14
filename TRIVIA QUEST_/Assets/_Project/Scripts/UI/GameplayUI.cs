using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System;

public class GameplayUI : MonoBehaviour
{
    public static event Action<int> OnAnswerSelected;

    [Header("UI Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject correctPanel;
    [SerializeField] private GameObject incorrectPanel;

    [Header("Timer Settings")]
    [SerializeField] private float timeLimit = 10f; // segundos por pregunta
    [SerializeField] private TMP_Text timerText;    // referencia al texto del tiempo
    private bool isTiming = false;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text currentPlayerText;
    [SerializeField] private TMP_Text currentPlayerScore;
    [SerializeField] private Button[] answerButtons;

    [Header("Feedback Colors")]
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color incorrectColor = Color.red;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private float feedbackDelay = 1.0f;

    private TMP_Text[] answerButtonTexts;
    private Image[] answerButtonImages; 
    private Question currentQuestion;

    private void Start()
    {
        answerButtonTexts = new TMP_Text[answerButtons.Length];
        answerButtonImages = new Image[answerButtons.Length];
        
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtonTexts[i] = answerButtons[i].GetComponentInChildren<TMP_Text>();
            answerButtonImages[i] = answerButtons[i].GetComponent<Image>();
            
            int buttonIndex = i;
            answerButtons[i].onClick.AddListener(() => SelectAnswer(buttonIndex));
        }

        gameplayPanel.SetActive(false);
        correctPanel.SetActive(false);
        incorrectPanel.SetActive(false);
    }

    public void DisplayQuestion(Question question, string playerName, int actualPlayer)
    {
        currentQuestion = question;
        
        questionText.text = currentQuestion.questionText;
        currentPlayerText.text = $"Turno de: {playerName}";
        currentPlayerScore.text = $"{actualPlayer}";


        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtonTexts[i].text = currentQuestion.answers[i];
            answerButtonImages[i].color = defaultColor;
            answerButtons[i].interactable = true;
        }
        isTiming = true;
        StartCoroutine(StartTimer());
        gameplayPanel.SetActive(true);
    }

    private void SelectAnswer(int selectedIndex)
    {
        isTiming = false;
        // Deshabilitar todos los botones para evitar múltiples clics
        foreach (var button in answerButtons)
        {
            button.interactable = false;
        }

        bool isCorrect = (selectedIndex == currentQuestion.correctAnswerIndex);
        
        StartCoroutine(ShowFeedbackAndNotify(selectedIndex, isCorrect));
    }

    private IEnumerator StartTimer()
    {
        float timeRemaining = timeLimit;

        while (timeRemaining > 0)
        {
            timerText.text = $"{Mathf.CeilToInt(timeRemaining)}s";
            yield return new WaitForSeconds(1f);

            // Si el jugador respondió, detener el temporizador
            if (!isTiming)
                yield break;

            timeRemaining -= 1f;
        }

        // Si el tiempo llega a 0
        TimeExpired();
    }
    private void TimeExpired()
    {
        isTiming = false;

        foreach (var button in answerButtons)
            button.interactable = false;

        // Mostrar visualmente la respuesta correcta
        answerButtonImages[currentQuestion.correctAnswerIndex].color = correctColor;

        StartCoroutine(ShowTimeoutFeedback());
    }

    private IEnumerator ShowTimeoutFeedback()
    {
        yield return new WaitForSeconds(feedbackDelay);

        gameplayPanel.SetActive(false);
        incorrectPanel.SetActive(true);
        timerText.text = "¡Tiempo agotado!";

        yield return new WaitForSeconds(feedbackDelay + 0.5f);

        incorrectPanel.SetActive(false);
        OnAnswerSelected?.Invoke(-1);
    }
    private IEnumerator ShowFeedbackAndNotify(int selectedIndex, bool isCorrect)
    {
        
        if (!isCorrect)
            answerButtonImages[selectedIndex].color = incorrectColor;

        answerButtonImages[currentQuestion.correctAnswerIndex].color = correctColor;

        
        yield return new WaitForSeconds(feedbackDelay);

        
        gameplayPanel.SetActive(false);

        
        if (isCorrect)
        {
            correctPanel.SetActive(true);
        }
        else
        {
            incorrectPanel.SetActive(true);
        }

        
        yield return new WaitForSeconds(feedbackDelay + 0.5f);

        
        correctPanel.SetActive(false);
        incorrectPanel.SetActive(false);

        
        OnAnswerSelected?.Invoke(selectedIndex);
    }

}