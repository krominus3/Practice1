using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("—сылки")]
    [SerializeField] private DiceSpawner diceSpawner;
    [SerializeField] private DiceInput diceInput;
    [SerializeField] private InputRebinder inputRebinder;

    private void OnEnable()
    {
        diceInput.OnThrowRequested += HandleThrowRequest;
    }

    private void OnDisable()
    {
        diceInput.OnThrowRequested -= HandleThrowRequest;
    }

    private void HandleThrowRequest()
    {
        diceSpawner.ThrowAllDice();
    }

    // === ѕубличные методы дл€ UI ===

    //UI Slider или InputField
    public void OnDiceCountChanged(int newCount)
    {
        diceSpawner.SetDiceCount(newCount);
    }

    //UI Button "»зменить клавишу"
    public void OnRebindButtonClicked()
    {
        inputRebinder.StartRebinding();
    }

    //UI Button "—бросить"
    public void OnResetBindingClicked()
    {
        inputRebinder.ResetBinding();
    }
}