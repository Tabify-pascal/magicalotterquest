
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private Image energyDigit;

    [Header("Digit Sprites (0-9)")]
    [SerializeField] private Sprite[] digits = new Sprite[10];

    private void Start()
    {
        energyManager.OnEnergyChanged += UpdateEnergy;

        UpdateEnergy(
            energyManager.CurrentEnergy,
            energyManager.MaxEnergy
        );
    }

    private void OnDestroy()
    {
        if (energyManager != null)
            energyManager.OnEnergyChanged -= UpdateEnergy;
    }

    private void UpdateEnergy(int current, int max)
    {
        if (current < 0 || current >= digits.Length)
            return;

        energyDigit.sprite = digits[current];
    }
}
