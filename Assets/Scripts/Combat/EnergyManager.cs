
using System;
using UnityEngine;

public class EnergyManager : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxEnergy = 3;

    public int MaxEnergy => maxEnergy;
    public int CurrentEnergy { get; private set; }

    public event Action<int, int> OnEnergyChanged;

    private void Awake()
    {
        CurrentEnergy = maxEnergy;
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

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);

        return true;
    }

    public void Refill()
    {
        CurrentEnergy = maxEnergy;

        OnEnergyChanged?.Invoke(CurrentEnergy, maxEnergy);
    }
}
