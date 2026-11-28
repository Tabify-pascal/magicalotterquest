using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Starting Deck")]
    [SerializeField]
    private List<StartingDeckEntry> startingDeck = new();

    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();

    public int DrawPileCount => drawPile.Count;
    public int DiscardPileCount => discardPile.Count;

    private void Awake()
    {
        InitializeDeck();
    }

    private void InitializeDeck()
    {
        drawPile.Clear();
        discardPile.Clear();

        foreach (StartingDeckEntry entry in startingDeck)
        {
            if (entry == null || entry.card == null)
                continue;

            for (int i = 0; i < entry.amount; i++)
            {
                drawPile.Add(
                    new CardInstance(
                        entry.card,
                        entry.isUpgraded
                    )
                );
            }
        }

        Shuffle(drawPile);

        Debug.Log(
            $"Deck initialized with {drawPile.Count} cards."
        );
    }

    public CardInstance DrawCard()
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0)
                return null;

            drawPile.AddRange(discardPile);
            discardPile.Clear();

            Shuffle(drawPile);
        }

        int lastIndex = drawPile.Count - 1;

        CardInstance card = drawPile[lastIndex];
        drawPile.RemoveAt(lastIndex);

        return card;
    }

    public void Discard(CardInstance card)
    {
        if (card == null)
            return;

        discardPile.Add(card);
    }

    private void Shuffle(List<CardInstance> cards)
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            (cards[i], cards[randomIndex]) =
                (cards[randomIndex], cards[i]);
        }
    }
}