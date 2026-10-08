using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        scoreManager.OnScoreCalculated += UpdateScore;
    }

    private void OnDisable()
    {
        scoreManager.OnScoreCalculated -= UpdateScore;
    }

    private void UpdateScore(int total, List<int> values)
    {
        scoreText.text = $"—ÛÏÏ‡: {total}\n({string.Join(" + ", values)})";
    }
}