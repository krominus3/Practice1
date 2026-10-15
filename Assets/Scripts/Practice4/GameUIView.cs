using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameUIView : MonoBehaviour
{
    [Header("Тексты")]
    [SerializeField] private TextMeshProUGUI diceValuesText;
    [SerializeField] private TextMeshProUGUI totalScoreText;
    [SerializeField] private TextMeshProUGUI drawThresholdText;
    [SerializeField] private TextMeshProUGUI winThresholdText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI diceCountText;

    [Header("Поля ввода")]
    [SerializeField] private TMP_InputField diceCountInput;

    [Header("Кнопки")]
    [SerializeField] private Button throwButton;

    private void Awake()
    {
        Debug.Assert(diceValuesText != null, "diceValuesText не назначен!");
        Debug.Assert(totalScoreText != null, "totalScoreText не назначен!");
        Debug.Assert(drawThresholdText != null, "drawThresholdText не назначен!");
        Debug.Assert(winThresholdText != null, "winThresholdText не назначен!");
        Debug.Assert(resultText != null, "resultText не назначен!");
        Debug.Assert(diceCountText != null, "diceCountText не назначен!");
        Debug.Assert(diceCountInput != null, "diceCountInput не назначен!");
        Debug.Assert(throwButton != null, "throwButton не назначен!");
    }

    private void Start()
    {
        SetDiceCount();

    }

    public void SetDiceValues(List<int> values)
    {
        if (values == null || values.Count == 0)
        {
            diceValuesText.text = "—";
            return;
        }
        diceValuesText.text = string.Join("  |  ", values);
    }

    public void SetTotalScore(int total)
    {
        totalScoreText.text = $"Сумма: {total}";
    }

    public void SetThresholds(int draw, int win)
    {
        drawThresholdText.text = $"Ничья от: {draw}";
        winThresholdText.text = $"Победа от: {win}";
    }

    public void SetResult(GameModel.RoundResult result)
    {
        switch (result)
        {
            case GameModel.RoundResult.Win:
                resultText.text = "(UwU) ПОБЕДА!";
                resultText.color = Color.green;
                break;
            case GameModel.RoundResult.Draw:
                resultText.text = "(-_-) НИЧЬЯ";
                resultText.color = Color.yellow;
                break;
            case GameModel.RoundResult.Loss:
                resultText.text = "(; _ ;) ПОРАЖЕНИЕ";
                resultText.color = Color.red;
                break;
            default:
                resultText.text = "—";
                resultText.color = Color.white;
                break;
        }
    }

    public void SetDiceCount(int count = 4)
    {
        diceCountText.text = $"Кубиков: {count}";

        if (!diceCountInput.isFocused)
        {
            diceCountInput.SetTextWithoutNotify(count.ToString());
        }
    }

    public Button ThrowButton => throwButton;
    public TMP_InputField DiceCountInput => diceCountInput;
}