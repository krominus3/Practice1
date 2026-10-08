using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DiceCountSlider : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int minDice = 1;
    [SerializeField] private int maxDice = 10;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.minValue = minDice;
        slider.maxValue = maxDice;
        slider.wholeNumbers = true;
        slider.value = 2;

        slider.onValueChanged.AddListener(OnSliderValueChanged);

        OnSliderValueChanged(slider.value);
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        int count = Mathf.RoundToInt(value);
        gameManager.OnDiceCountChanged(count);
    }
}