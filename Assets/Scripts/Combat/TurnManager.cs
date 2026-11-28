using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum TurnState
{
    Player,
    Enemy,
    Victory,
    Defeat
}

public class TurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private CardTargetingController targetingController;
    [SerializeField] private HandManager handManager;
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private Combatant player;
    [SerializeField] private Combatant enemy;

    [Header("Animations")]
    [SerializeField] private CombatAnimator playerAnimator;
    [SerializeField] private CombatAnimator enemyAnimator;

    [Header("UI")]
    [SerializeField] private Button endTurnButton;
    [SerializeField] private EnemyIntentContainer enemyIntentUI;

    [Header("Turn Settings")]
    [SerializeField, Min(1)] private int cardsPerTurn = 5;

    public TurnState CurrentState { get; private set; }
    public int TurnNumber { get; private set; }

    public bool IsPlayerTurn =>
        CurrentState == TurnState.Player;

    public bool IsCombatOver =>
        CurrentState == TurnState.Victory ||
        CurrentState == TurnState.Defeat;

    private void OnEnable()
    {
        player.OnDeath += HandlePlayerDeath;
        enemy.OnDeath += HandleEnemyDeath;
    }

    private void OnDisable()
    {
        player.OnDeath -= HandlePlayerDeath;
        enemy.OnDeath -= HandleEnemyDeath;
    }

    private void Start()
    {
        StartPlayerTurn();
    }

    public void EndPlayerTurn()
    {
        if (!IsPlayerTurn)
            return;

        CurrentState = TurnState.Enemy;

        endTurnButton.interactable = false;

        targetingController.CancelSelection();
        handManager.DiscardHand();

        Debug.Log($"Player turn {TurnNumber} ended.");

        StartCoroutine(ExecuteEnemyTurn());
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        Debug.Log("Enemy turn started.");

        yield return enemyController.ExecuteTurn();

        if (IsCombatOver)
            yield break;

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        if (IsCombatOver)
            return;

        TurnNumber++;
        CurrentState = TurnState.Player;

        player.ClearBlock();

        energyManager.Refill();
        handManager.DrawCards(cardsPerTurn);

        enemyController.PrepareIntent();

        endTurnButton.interactable = true;

        Debug.Log(
            $"Player turn {TurnNumber} started. " +
            $"Cards in hand: {handManager.CardCount}"
        );
    }

    private void HandleEnemyDeath()
    {
        if (IsCombatOver)
            return;

        CurrentState = TurnState.Victory;

        FinishCombat();

        enemyAnimator.PlayDefeat();
        playerAnimator.PlayVictory();

        Debug.Log("VICTORY!");
    }

    private void HandlePlayerDeath()
    {
        if (IsCombatOver)
            return;

        CurrentState = TurnState.Defeat;

        FinishCombat();

        playerAnimator.PlayDefeat();

        Debug.Log("DEFEAT!");
    }

    private void FinishCombat()
    {
        StopAllCoroutines();

        targetingController.CancelSelection();

        enemyIntentUI.Clear();

        endTurnButton.interactable = false;
    }
}