
using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [Header("Energy Settings")]
    [SerializeField, Min(0)] private int turnEnergy = 3;
    [SerializeField, Min(1)] private int maxEnergy = 6;

    public int TurnEnergy => turnEnergy;
    public int MaxEnergy => maxEnergy;
    public int CurrentEnergy { get; private set; }

    public event Action<int, int> OnEnergyChanged;

    private void Awake()
    {
        CurrentEnergy = Mathf.Min(turnEnergy, maxEnergy);
    }

    public bool CanSpend(int amount)
    {
        return amount >= 0 && CurrentEnergy >= amount;
    }

    public bool TrySpend(int amount)
    {
        if (!CanSpend(amount))
            return false;

        CurrentEnergy -= amount;
        NotifyEnergyChanged();

        return true;
    }

    public void GainEnergy(int amount)
    {
        if (amount <= 0)
            return;

        CurrentEnergy = Mathf.Min(
            CurrentEnergy + amount,
            maxEnergy
        );

        NotifyEnergyChanged();
    }

    public void Refill()
    {
        CurrentEnergy = Mathf.Min(turnEnergy, maxEnergy);

        NotifyEnergyChanged();
    }

    private void NotifyEnergyChanged()
    {
        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }
}
