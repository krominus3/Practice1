using UnityEngine;

public class GameUIBinder : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] private GameModel model;
    [SerializeField] private GameUIView view;
    [SerializeField] private GameController controller;

    private void OnEnable()
    {
        model.OnDiceCountChanged += view.SetDiceCount;
        model.OnScoreUpdated += HandleScoreUpdated;
        model.OnThresholdsChanged += view.SetThresholds;
        model.OnResultChanged += view.SetResult;

        view.ThrowButton.onClick.AddListener(controller.OnThrowButtonClicked);
        view.DiceCountInput.onValueChanged.AddListener(controller.OnDiceCountInputChanged);

        InitializeView();
    }

    private void OnDisable()
    {
        model.OnDiceCountChanged -= view.SetDiceCount;
        model.OnScoreUpdated -= HandleScoreUpdated;
        model.OnThresholdsChanged -= view.SetThresholds;
        model.OnResultChanged -= view.SetResult;

        view.ThrowButton.onClick.RemoveListener(controller.OnThrowButtonClicked);
        view.DiceCountInput.onValueChanged.RemoveListener(controller.OnDiceCountInputChanged);
    }

    private void InitializeView()
    {
        view.SetDiceCount(model.DiceCount);
        view.SetThresholds(model.DrawThreshold, model.WinThreshold);
        view.SetResult(GameModel.RoundResult.None);
        view.SetDiceValues(null);
        view.SetTotalScore(0);
    }

    private void HandleScoreUpdated(System.Collections.Generic.List<int> values, int total)
    {
        view.SetDiceValues(values);
        view.SetTotalScore(total);
    }
}