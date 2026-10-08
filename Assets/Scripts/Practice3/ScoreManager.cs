using UnityEngine;
using System.Collections.Generic;
using System;

public class ScoreManager : MonoBehaviour
{
    [Header("Ссылки")]
    [SerializeField] private DiceSpawner diceSpawner;

    // НОВОЕ: событие, когда все очки посчитаны (для UI)
    public event Action<int, List<int>> OnScoreCalculated;

    // Храним результаты текущего броска: Dice -> Value
    private Dictionary<Dice, int> currentResults = new Dictionary<Dice, int>();

    private void OnEnable()
    {
        diceSpawner.OnDiceSpawned += HandleDiceSpawned;
        diceSpawner.OnAllDiceCleared += HandleAllDiceCleared;
    }

    private void OnDisable()
    {
        diceSpawner.OnDiceSpawned -= HandleDiceSpawned;
        diceSpawner.OnAllDiceCleared -= HandleAllDiceCleared;
    }

    private void HandleDiceSpawned(Dice dice)
    {
        // Подписываемся на результат каждого нового кубика
        dice.OnDiceResult += HandleDiceResult;
    }

    private void HandleAllDiceCleared()
    {
        // Отписываемся от всех кубиков и чистим результаты
        foreach (var dice in diceSpawner.SpawnedDice)
        {
            if (dice != null) dice.OnDiceResult -= HandleDiceResult;
        }
        currentResults.Clear();
    }

    private void HandleDiceResult(Dice dice, int value)
    {
        // Сохраняем результат
        currentResults[dice] = value;

        // Проверяем, все ли кубики остановились
        CheckAllDiceSettled();
    }

    private void CheckAllDiceSettled()
    {
        // Если есть хоть один не остановившийся кубик — ждем
        foreach (var dice in diceSpawner.SpawnedDice)
        {
            if (dice != null && !dice.IsSettled) return;
        }

        // Все остановились — считаем сумму
        CalculateTotalScore();
    }

    private void CalculateTotalScore()
    {
        int totalScore = 0;
        List<int> values = new List<int>();

        foreach (var kvp in currentResults)
        {
            totalScore += kvp.Value;
            values.Add(kvp.Value);
        }

        Debug.Log($"<color=cyan>=== ОБЩИЙ СЧЁТ: {totalScore} ===</color>");
        Debug.Log($"Значения: {string.Join(", ", values)}");

        // Сообщаем наружу (для UI)
        OnScoreCalculated?.Invoke(totalScore, values);
    }
}