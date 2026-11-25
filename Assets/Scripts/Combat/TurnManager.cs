
using System.Collections;
using UnityEngine;

public enum TurnState
{
    Player,
    Enemy
}

public class TurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private CardTargetingController targetingController;
    [SerializeField] private HandManager handManager;
    [SerializeField] private EnemyController enemyController;

    [Header("Turn Settings")]
    [SerializeField, Min(1)] private int cardsPerTurn = 5;

    public TurnState CurrentState { get; private set; }
    public int TurnNumber { get; private set; }

    public bool IsPlayerTurn =>
        CurrentState == TurnState.Player;

    private void Start()
    {
        StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        if (!IsPlayerTurn)
            return;

        CurrentState = TurnState.Enemy;

        // Cancel any selected card before destroying it.
        targetingController.CancelSelection();

        // Discard all remaining cards.
        handManager.DiscardHand();

        Debug.Log($"Player turn {TurnNumber} ended.");

        StartCoroutine(ExecuteEnemyTurn());
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        Debug.Log("Enemy turn started.");

        yield return enemyController.ExecuteTurn();

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        TurnNumber++;
        CurrentState = TurnState.Player;

        // Restore energy to 3.
        energyManager.Refill();

        // Draw a fresh hand.
        handManager.DrawCards(cardsPerTurn);

        enemyController.PrepareIntent();

        Debug.Log(
            $"Player turn {TurnNumber} started. " +
            $"Cards in hand: {handManager.CardCount}"
        );
    }
}
