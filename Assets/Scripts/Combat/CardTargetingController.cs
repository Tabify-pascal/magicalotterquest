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
    [SerializeField] private Combatant player;
    [SerializeField] private HandManager handManager;

    private CardView selectedCard;

    public void SelectCard(CardView card)
    {
        if (!turnManager.IsPlayerTurn || !player.IsAlive)
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

        // Self-targeting cards are played immediately.
        if (card.Card.TargetType == CardTargetType.Self)
        {
            PlayCard(player);
            return;
        }

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

        if (target == null || !target.IsAlive)
            return;

        PlayCard(target);
    }

    private void PlayCard(Combatant target)
    {
        if (!turnManager.IsPlayerTurn || !player.IsAlive)
            return;

        if (selectedCard == null || selectedCard.Card == null)
            return;

        if (target == null || !target.IsAlive)
            return;

        CardView playedCard = selectedCard;
        CardInstance card = playedCard.Card;

        if (!energyManager.TrySpend(card.EnergyCost))
        {
            CancelSelection();
            Debug.Log("Not enough energy!");
            return;
        }

        // Play the appropriate player animation.
        if (card.Damage > 0)
        {
            playerAnimator.PlaySpell();
        }
        else if (card.Block > 0)
        {
            playerAnimator.PlayBuff();
        }
        else
        {
            playerAnimator.PlaySpell();
        }

        // Apply damage to the target.
        if (card.Damage > 0)
        {
            int previousHealth = target.CurrentHealth;

            target.TakeDamage(card.Damage);

            if (target.IsAlive &&
                target.CurrentHealth < previousHealth &&
                target.TryGetComponent(
                    out CombatAnimator targetAnimator))
            {
                targetAnimator.PlayHurt();
            }
        }

        // Apply block to the player.
        if (card.Block > 0)
        {
            player.GainBlock(card.Block);
        }

        Debug.Log(
            $"Played {card.Name}: " +
            $"{card.Damage} damage, " +
            $"{card.Block} block."
        );


        CancelSelection();

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