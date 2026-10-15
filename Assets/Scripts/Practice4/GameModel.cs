using UnityEngine;
using System;
using System.Collections.Generic;

public class GameModel : MonoBehaviour
{
    [Header("Настройки по умолчанию")]
    [SerializeField] private int defaultDiceCount = 2;
    [SerializeField] private int minDiceCount = 1;
    [SerializeField] private int maxDiceCount = 6;

    public int DiceCount { get; private set; }
    public int TotalScore { get; private set; }
    public List<int> DiceValues { get; private set; } = new List<int>();

    public int DrawThreshold { get; private set; }
    public int WinThreshold { get; private set; }

    public enum RoundResult { None, Loss, Draw, Win }
    public RoundResult CurrentResult { get; private set; } = RoundResult.None;

    public event Action<int> OnDiceCountChanged;
    public event Action<List<int>, int> OnScoreUpdated; // (значения, сумма)
    public event Action<int, int> OnThresholdsChanged;  // (ничья, победа)
    public event Action<RoundResult> OnResultChanged;

    private void Awake()
    {
        DiceCount = defaultDiceCount;
        DiceValues = new List<int>();
        TotalScore = 0;
        GenerateThresholds();

        Debug.Log($"[GameModel] Инициализирована. Кубиков: {DiceCount}, Ничья: {DrawThreshold}, Победа: {WinThreshold}");
    }

    public bool TrySetDiceCount(int count)
    {
        if (count < minDiceCount || count > maxDiceCount) return false;

        DiceCount = count;
        OnDiceCountChanged?.Invoke(DiceCount);
        return true;
    }

    public void GenerateThresholds()
    {
        int minPossible = DiceCount * 1;
        int maxPossible = DiceCount * 6;
        int range = maxPossible - minPossible;

        int middle = (minPossible + maxPossible) / 2;
        DrawThreshold = middle;

        WinThreshold = middle + Mathf.RoundToInt(range * UnityEngine.Random.Range(0.15f, 0.25f));

        OnThresholdsChanged?.Invoke(DrawThreshold, WinThreshold);
    }

    public void ApplyRollResult(List<int> values)
    {
        DiceValues = new List<int>(values);
        TotalScore = 0;
        foreach (var v in values) TotalScore += v;

        if (TotalScore >= WinThreshold) CurrentResult = RoundResult.Win;
        else if (TotalScore >= DrawThreshold) CurrentResult = RoundResult.Draw;
        else CurrentResult = RoundResult.Loss;

        OnScoreUpdated?.Invoke(DiceValues, TotalScore);
        OnResultChanged?.Invoke(CurrentResult);

        Debug.Log($"[GameModel] Бросок: {string.Join(", ", DiceValues)} = {TotalScore}. Результат: {CurrentResult}");
    }

    public void StartNewRound()
    {
        GenerateThresholds();
        CurrentResult = RoundResult.None;
        OnResultChanged?.Invoke(CurrentResult);
    }
}