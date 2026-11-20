
using System.Collections;
using UnityEngine;

public enum TurnState
{
    Player,
    Enemy
}

public class TurnManager : MonoBehaviour
{
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private CardTargetingController targetingController;

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

        targetingController.CancelSelection();

        Debug.Log("Player turn ended.");

        StartCoroutine(ExecuteEnemyTurn());
    }

    private IEnumerator ExecuteEnemyTurn()
    {
        Debug.Log("Enemy turn started.");

        // Temporary placeholder for enemy actions.
        yield return new WaitForSeconds(1f);

        Debug.Log("Ogre takes its turn!");

        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        TurnNumber++;
        CurrentState = TurnState.Player;

        energyManager.Refill();

        Debug.Log($"Player turn {TurnNumber} started.");
    }
}
