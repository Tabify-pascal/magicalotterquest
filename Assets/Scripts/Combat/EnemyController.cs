
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Combatant))]
[RequireComponent(typeof(CombatAnimator))]
public class EnemyController : MonoBehaviour
{
    [SerializeField, Min(0)] private int attackDamage = 2;
    [SerializeField] private Combatant player;
    [SerializeField] private CombatAnimator playerAnimator;
    [SerializeField] private EnemyIntentContainer intentUI;

    private readonly List<EnemyIntent> currentIntents = new();

    private Combatant combatant;
    private CombatAnimator combatAnimator;

    private void Awake()
    {
        combatant = GetComponent<Combatant>();
        combatAnimator = GetComponent<CombatAnimator>();
    }


    public IEnumerator ExecuteTurn()
    {
        intentUI.Clear();

        if (!combatant.IsAlive)
            yield break;

        // Clear remaining block at the start of the enemy turn.
        combatant.ClearBlock();

        foreach (EnemyIntent intent in currentIntents)
        {
            if (!combatant.IsAlive || !player.IsAlive)
                break;

            switch (intent.Type)
            {
                case EnemyIntentType.Attack:
                    yield return ExecuteAttack(intent.Value);
                    break;

                case EnemyIntentType.Block:
                    combatant.GainBlock(intent.Value);

                    Debug.Log(
                        $"{name} gains {intent.Value} Block."
                    );

                    yield return new WaitForSeconds(0.4f);
                    break;
            }
        }
    }


    public void PrepareIntent()
    {
        currentIntents.Clear();

        currentIntents.Add(
            new EnemyIntent(EnemyIntentType.Attack, attackDamage)
        );

        currentIntents.Add(
            new EnemyIntent(EnemyIntentType.Block, 3)
        );

        intentUI.ShowIntents(currentIntents);
    }


    private IEnumerator ExecuteAttack(int damage)
    {
        yield return new WaitForSeconds(0.4f);

        combatAnimator.PlayAttack();

        yield return new WaitForSeconds(0.4f);

        if (!combatant.IsAlive || !player.IsAlive)
            yield break;

        int previousHealth = player.CurrentHealth;

        player.TakeDamage(damage);

        if (player.IsAlive &&
            player.CurrentHealth < previousHealth)
        {
            playerAnimator.PlayHurt();
        }

        Debug.Log($"{name} attacks for {damage} damage!");

        yield return new WaitForSeconds(0.5f);
    }

}
