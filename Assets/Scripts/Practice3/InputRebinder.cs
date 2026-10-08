using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputRebinder : MonoBehaviour
{
    [Header("Действие для переназначения")]
    [SerializeField] private InputActionReference throwAction;

    [Header("Настройки")]
    [SerializeField] private bool saveToPlayerPrefs = true;
    [SerializeField] private string bindingKey = "ThrowBinding";

    public event Action<string> OnBindingChanged;

    private void Start()
    {
        LoadBinding();
    }

    public void StartRebinding()
    {
        if (throwAction == null || throwAction.action == null)
        {
            Debug.LogError("Throw Action не назначен!");
            return;
        }

        throwAction.action.Disable();

        var rebindOperation = throwAction.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse") // Исключаем мышь
            .WithCancelingThrough("<Keyboard>/escape") // Отмена по Escape
            .OnMatchWaitForAnother(0.1f) // Ждать 0.1 сек чтобы отсеять случайные нажатия
            .OnComplete(operation => OnRebindComplete(operation))
            .OnCancel(operation => OnRebindCanceled(operation));

        rebindOperation.Start();
    }

    private void OnRebindComplete(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();

        string newBinding = GetCurrentBindingName();

        Debug.Log($"<color=cyan>Новая клавиша броска: {newBinding}</color>");

        if (saveToPlayerPrefs)
        {
            SaveBinding();
        }

        throwAction.action.Enable();

        OnBindingChanged?.Invoke(newBinding);
    }

    private void OnRebindCanceled(InputActionRebindingExtensions.RebindingOperation operation)
    {
        operation.Dispose();
        throwAction.action.Enable();
        Debug.Log("Переназначение отменено");
    }

    // Получить читаемое имя текущей клавиши
    public string GetCurrentBindingName()
    {
        if (throwAction == null || throwAction.action == null) return "None";

        int bindingIndex = throwAction.action.GetBindingIndexForControl(throwAction.action.controls[0]);

        // Получаем читаемое имя
        string displayName = InputControlPath.ToHumanReadableString(
            throwAction.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );

        return displayName;
    }

    // === Сохранение / Загрузка ===

    private void SaveBinding()
    {
        string bindingJson = throwAction.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(bindingKey, bindingJson);
        PlayerPrefs.Save();
    }

    private void LoadBinding()
    {
        if (!saveToPlayerPrefs) return;
        if (!PlayerPrefs.HasKey(bindingKey)) return;

        string bindingJson = PlayerPrefs.GetString(bindingKey);
        throwAction.action.LoadBindingOverridesFromJson(bindingJson);

        Debug.Log($"Загружена привязка: {GetCurrentBindingName()}");
    }

    // Сброс к дефолтной привязке
    public void ResetBinding()
    {
        throwAction.action.RemoveAllBindingOverrides();
        PlayerPrefs.DeleteKey(bindingKey);

        Debug.Log($"<color=yellow>Привязка сброшена: {GetCurrentBindingName()}</color>");
        OnBindingChanged?.Invoke(GetCurrentBindingName());
    }
}