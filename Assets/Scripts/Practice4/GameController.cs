using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] private GameModel model;
    [SerializeField] private DiceSpawner diceSpawner;
    [SerializeField] private ScoreManager scoreManager;

    private bool waitingForRoll = false;

    private void OnEnable()
    {
        scoreManager.OnScoreCalculated += HandleScoreCalculated;
    }

    private void OnDisable()
    {
        scoreManager.OnScoreCalculated -= HandleScoreCalculated;
    }

    public void OnThrowButtonClicked()
    {
        if (waitingForRoll) return;

        model.StartNewRound();

        // Ѕросаем кубики
        diceSpawner.ThrowAllDice();
        waitingForRoll = true;

        Debug.Log("[GameController]  убики брошены, ждЄм результата...");
    }

    public void OnDiceCountInputChanged(string input)
    {
        if (int.TryParse(input, out int count))
        {
            if (model.TrySetDiceCount(count))
            {
                diceSpawner.SetDiceCount(count);
            }
        }
    }

    private void HandleScoreCalculated(int total, System.Collections.Generic.List<int> values)
    {
        if (!waitingForRoll) return;

        waitingForRoll = false;
        model.ApplyRollResult(values);
    }
}