
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private CardData startingCard;
    [SerializeField, Min(1)] private int startingDeckSize = 10;

    private readonly List<CardInstance> drawPile = new();
    private readonly List<CardInstance> discardPile = new();

    public int DrawPileCount => drawPile.Count;
    public int DiscardPileCount => discardPile.Count;

    private void Awake()
    {
        for (int i = 0; i < startingDeckSize; i++)
        {
            drawPile.Add(new CardInstance(startingCard));
        }

        Shuffle(drawPile);
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
