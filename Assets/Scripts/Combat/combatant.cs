
using System;
using UnityEngine;

public class Combatant : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 10;

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public int CurrentBlock { get; private set; }

    public bool IsAlive => CurrentHealth > 0;

    public event Action<int, int> OnHealthChanged;
    public event Action<int> OnBlockChanged;
    public event Action OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
        CurrentBlock = 0;
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || !IsAlive)
            return;

        // Absorb damage with block.
        int absorbedDamage = Mathf.Min(CurrentBlock, amount);

        if (absorbedDamage > 0)
        {
            CurrentBlock -= absorbedDamage;
            OnBlockChanged?.Invoke(CurrentBlock);
        }

        int remainingDamage = amount - absorbedDamage;

        if (remainingDamage <= 0)
            return;

        // Apply remaining damage to health.
        CurrentHealth = Mathf.Max(
            0,
            CurrentHealth - remainingDamage
        );

        OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

        if (CurrentHealth == 0)
            OnDeath?.Invoke();
    }

    public void GainBlock(int amount)
    {
        if (amount <= 0 || !IsAlive)
            return;

        CurrentBlock += amount;

        OnBlockChanged?.Invoke(CurrentBlock);
    }

    public void ClearBlock()
    {
        if (CurrentBlock == 0)
            return;

        CurrentBlock = 0;

        OnBlockChanged?.Invoke(CurrentBlock);
    }
}
