
using System.Collections.Generic;
using UnityEngine;

public class HandManager : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private CardView cardPrefab;
    [SerializeField] private Transform handContainer;
    [SerializeField] private CardTargetingController targetingController;

    private readonly List<CardView> cardsInHand = new();

    public int CardCount => cardsInHand.Count;

    public void DrawCards(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CardInstance card = deckManager.DrawCard();

            if (card == null)
                break;

            CardView cardView =
            Instantiate(cardPrefab, handContainer);

            cardView.SetCard(card);

            cardView
                .GetComponent<CardPlayTest>()
                .Initialize(targetingController);

            cardsInHand.Add(cardView);
        }
    }

    public void PlayCard(CardView cardView)
    {
        if (!cardsInHand.Remove(cardView))
            return;

        deckManager.Discard(cardView.Card);

        Destroy(cardView.gameObject);
    }

    public void DiscardHand()
    {
        foreach (CardView cardView in cardsInHand)
        {
            deckManager.Discard(cardView.Card);
            Destroy(cardView.gameObject);
        }

        cardsInHand.Clear();
    }

}
