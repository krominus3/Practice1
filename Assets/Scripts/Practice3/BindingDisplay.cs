using UnityEngine;
using TMPro;

public class BindingDisplay : MonoBehaviour
{
    [SerializeField] private InputRebinder rebinder;
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        rebinder.OnBindingChanged += UpdateText;
        UpdateText(rebinder.GetCurrentBindingName());
    }

    private void OnDisable()
    {
        rebinder.OnBindingChanged -= UpdateText;
    }

    private void UpdateText(string bindingName)
    {
        text.text = $"Бросок: {bindingName}";
    }
}