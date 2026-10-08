using UnityEngine;
using System; // Для Action

[RequireComponent(typeof(DicePhysics))]
[RequireComponent(typeof(DiceResult))]
public class Dice : MonoBehaviour
{
    private DicePhysics physics;
    private DiceResult result;

    public event Action<Dice, int> OnDiceResult;

    public bool IsSettled { get; private set; }

    private void Awake()
    {
        physics = GetComponent<DicePhysics>();
        result = GetComponent<DiceResult>();
    }

    private void OnEnable()
    {
        result.OnResultCalculated += HandleResult;
    }

    private void OnDisable()
    {
        result.OnResultCalculated -= HandleResult;
    }

    public void Throw()
    {
        IsSettled = false;
        physics.Throw();
        result.StartChecking();
    }

    private void HandleResult(int value)
    {
        IsSettled = true;
        Debug.Log($"<color=green>Кубик {gameObject.name} показал: {value}</color>");

        OnDiceResult?.Invoke(this, value);
    }
}