
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CardView))]
public class CardPlayTest : MonoBehaviour
{
    private Button button;
    private CardView cardView;
    private CardTargetingController targetingController;

    private void Awake()
    {
        button = GetComponent<Button>();
        cardView = GetComponent<CardView>();

        button.onClick.AddListener(SelectCard);
    }

    public void Initialize(CardTargetingController controller)
    {
        targetingController = controller;
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(SelectCard);
    }

    private void SelectCard()
    {
        if (targetingController == null)
        {
            Debug.LogWarning("Card has no targeting controller!");
            return;
        }

        targetingController.SelectCard(cardView);
    }
}
