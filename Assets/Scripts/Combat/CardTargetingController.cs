
using UnityEngine;
using UnityEngine.InputSystem;

public class CardTargetingController : MonoBehaviour
{
    [SerializeField] private Camera combatCamera;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private TurnManager turnManager;

    private CardView selectedCard;

    public void SelectCard(CardView card)
    {
        if (!turnManager.IsPlayerTurn)
            return;

        if (card == null || card.Card == null)
            return;

        if (selectedCard == card)
        {
            selectedCard = null;
            Debug.Log("Card selection cancelled.");
            return;
        }

        if (!energyManager.CanSpend(card.Card.EnergyCost))
        {
            Debug.Log("Not enough energy!");
            return;
        }

        selectedCard = card;

        Debug.Log($"Selected {card.Card.Name}");
    }

    private void Update()
    {
        if (selectedCard == null)
            return;

        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector3 worldPosition =
            combatCamera.ScreenToWorldPoint(mousePosition);

        Collider2D hit = Physics2D.OverlapPoint(
            new Vector2(worldPosition.x, worldPosition.y),
            enemyLayer
        );

        if (hit == null)
            return;

        Combatant target = hit.GetComponentInParent<Combatant>();

        if (target == null)
            return;

        PlayCard(target);
    }

    private void PlayCard(Combatant target)
    {
        if (!turnManager.IsPlayerTurn)
        return;
        
        CardInstance card = selectedCard.Card;

        if (!energyManager.TrySpend(card.EnergyCost))
        {
            selectedCard = null;
            Debug.Log("Not enough energy!");
            return;
        }

        target.TakeDamage(card.Damage);

        Debug.Log(
            $"Played {card.Name} on {target.name} " +
            $"for {card.Damage} damage! " +
            $"Energy: {energyManager.CurrentEnergy}/" +
            $"{energyManager.MaxEnergy}"
        );

        selectedCard = null;
    }

    public void CancelSelection()
    {
        selectedCard = null;
    }
}
