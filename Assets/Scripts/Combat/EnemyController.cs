
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
        if (currentIntents.Count == 0)
            yield break;

        intentUI.Clear();

        if (combatant.CurrentHealth <= 0)
            yield break;

        // Brief pause before attacking.
        yield return new WaitForSeconds(0.4f);

        combatAnimator.PlayAttack();

        // Wait for the attack animation to progress.
        yield return new WaitForSeconds(0.4f);

        if (combatant.CurrentHealth <= 0 ||
            player.CurrentHealth <= 0)
            yield break;

        player.TakeDamage(attackDamage);

        if (player.CurrentHealth > 0)
            playerAnimator.PlayHurt();

        Debug.Log(
            $"{name} attacks for {attackDamage} damage!"
        );

        yield return new WaitForSeconds(0.5f);
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
}
