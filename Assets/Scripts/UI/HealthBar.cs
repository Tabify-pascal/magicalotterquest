
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Combatant combatant;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        combatant.OnHealthChanged += UpdateHealth;
        UpdateHealth(combatant.CurrentHealth, combatant.MaxHealth);
    }

    private void OnDestroy()
    {
        if (combatant != null)
            combatant.OnHealthChanged -= UpdateHealth;
    }

    private void UpdateHealth(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
        healthText.text = $"{current}/{max}";
    }
}
