
using UnityEngine;
using UnityEngine.InputSystem;

public class CardTargetingController : MonoBehaviour
{
    [SerializeField] private Camera combatCamera;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private EnergyManager energyManager;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private TargetingArrowUI targetingArrow;
    [SerializeField] private RectTransform gameCanvasRect;
    [SerializeField] private CombatAnimator playerAnimator;
    [SerializeField] private HandManager handManager;

    private CardView selectedCard;

    public void SelectCard(CardView card)
    {
        if (!turnManager.IsPlayerTurn)
            return;

        if (card == null || card.Card == null)
            return;

        if (selectedCard == card)
        {
            CancelSelection();
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

        UpdateArrow();

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

        if (selectedCard == null || selectedCard.Card == null)
            return;

        CardInstance card = selectedCard.Card;

        if (!energyManager.TrySpend(card.EnergyCost))
        {
            CancelSelection();
            Debug.Log("Not enough energy!");
            return;
        }

        // Player casts the spell.
        playerAnimator.PlaySpell();

        // Apply damage.
        target.TakeDamage(card.Damage);

        // Enemy reacts to damage.
        if (target.CurrentHealth > 0 &&
            target.TryGetComponent(out CombatAnimator targetAnimator))
        {
            targetAnimator.PlayHurt();
        }

        Debug.Log(
            $"Played {card.Name} on {target.name} " +
            $"for {card.Damage} damage!"
        );

        // Save the selected card before clearing the selection
        CardView playedCard = selectedCard;

        // Hide the targeting arrow and clear the selection.
        CancelSelection();

        // Move the card to the discard pile.
        handManager.PlayCard(playedCard);
    }

    public void CancelSelection()
    {
        selectedCard = null;
        targetingArrow.Hide();
    }

    private void UpdateArrow()
    {
        if (selectedCard == null || Mouse.current == null)
            return;

        RectTransform cardRect =
            selectedCard.GetComponent<RectTransform>();

        Vector3[] corners = new Vector3[4];
        cardRect.GetWorldCorners(corners);

        Vector2 startScreen = new Vector2(
            (corners[1].x + corners[2].x) * 0.5f,
            (corners[1].y + corners[2].y) * 0.5f
        );

        Vector2 mouseScreen = Mouse.current.position.ReadValue();

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gameCanvasRect,
            startScreen,
            null,
            out Vector2 startLocal
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            gameCanvasRect,
            mouseScreen,
            null,
            out Vector2 mouseLocal
        );

        targetingArrow.Show(startLocal, mouseLocal);
    }
}
