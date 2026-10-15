using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class DiceInput : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] private InputActionReference throwAction;
    [SerializeField] private GameController GC;

    public event Action OnThrowRequested;

    private void OnEnable()
    {
        throwAction.action.Enable();
        throwAction.action.performed += HandleInput;
    }

    private void OnDisable()
    {
        throwAction.action.performed -= HandleInput;
        throwAction.action.Disable();
    }

    private void HandleInput(InputAction.CallbackContext context)
    {
        // Просто кричим: "Эй, меня бросили!67"
        OnThrowRequested?.Invoke();
    }
}