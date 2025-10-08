using UnityEngine;
using UnityEditor;
using System.IO;

public class QuestionImporter
{
    private static string questionsAssetPath = "Assets/_Project/ScriptableObjects/Questions";

    [MenuItem("Trivia Quest/Import Questions from CSV")]
    public static void ImportQuestions()
    {
        string filePath = EditorUtility.OpenFilePanel("Select Question CSV", "", "csv");
        if (string.IsNullOrEmpty(filePath)) return;

        string[] lines = File.ReadAllLines(filePath);

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(';');

            if (values.Length != 6)
            {
                Debug.LogWarning($"Skipping line {i}: Incorrect number of values.");
                continue;
            }

            Question questionAsset = ScriptableObject.CreateInstance<Question>();
            
            questionAsset.questionText = values[0];
            questionAsset.answers[0] = values[1];
            questionAsset.answers[1] = values[2];
            questionAsset.answers[2] = values[3];
            questionAsset.answers[3] = values[4];
            questionAsset.correctAnswerIndex = int.Parse(values[5]);

            string assetName = $"Question_{i.ToString("000")}.asset";
            string assetPath = Path.Combine(questionsAssetPath, assetName);
            
            AssetDatabase.CreateAsset(questionAsset, assetPath);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        EditorUtility.DisplayDialog("Import Complete", $"Successfully imported {lines.Length - 1} questions.", "OK");
    }
}