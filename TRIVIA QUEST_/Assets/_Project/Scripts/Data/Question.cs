using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Trivia Quest/Question", order = 1)]
public class Question : ScriptableObject
{
    [TextArea(3, 10)]
    public string questionText;
    
    public string[] answers = new string[4];
    
    [Range(0, 3)]
    public int correctAnswerIndex;
}