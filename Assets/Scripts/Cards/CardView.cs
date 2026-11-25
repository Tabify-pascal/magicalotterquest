
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [Header("Card Data")]
    [SerializeField] private CardData startingCard;
    [SerializeField] private bool startUpgraded;

    [Header("Artwork")]
    [SerializeField] private Image artworkImage;

    [Header("Card Frames")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Sprite normalFrame;
    [SerializeField] private Sprite upgradedFrame;

    [Header("Card Text")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private TMP_Text descriptionText;

    public CardInstance Card { get; private set; }

    private void Awake()
    {
        if (startingCard != null)
            SetCard(new CardInstance(startingCard, startUpgraded));
    }

    public void SetCard(CardInstance card)
    {
        Card = card;
        Refresh();
    }

    public void Refresh()
    {
        if (Card == null)
            return;

        // Artwork
        artworkImage.sprite = Card.Data.artwork;

        // Normal or upgraded frame
        frameImage.sprite = Card.IsUpgraded
            ? upgradedFrame
            : normalFrame;

        // Card text
        nameText.text = Card.Name;
        energyText.text = Card.EnergyCost.ToString();
        descriptionText.text = $"Deal {Card.Damage} damage.";
    }
}
