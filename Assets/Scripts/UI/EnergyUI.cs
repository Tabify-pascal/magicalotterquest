
using TMPro;
using UnityEngine;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private TMP_Text energyText;

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
        energyText.text = $"{current}/{max}";
    }
}
