
using UnityEngine;
using UnityEngine.InputSystem;

public class CardTargetingController : MonoBehaviour
{
    [SerializeField] private Camera combatCamera;
    [SerializeField] private LayerMask enemyLayer;

    private CardView selectedCard;

    public void SelectCard(CardView card)
    {
        if (selectedCard == card)
        {
            selectedCard = null;
            Debug.Log("Card selection cancelled.");
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
        CardInstance card = selectedCard.Card;

        target.TakeDamage(card.Damage);

        Debug.Log(
            $"Played {card.Name} on {target.name} " +
            $"for {card.Damage} damage!"
        );

        selectedCard = null;
    }
}
