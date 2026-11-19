
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(CardView))]
public class CardPlayTest : MonoBehaviour
{
    [SerializeField] private CardTargetingController targetingController;

    private Button button;
    private CardView cardView;

    private void Awake()
    {
        button = GetComponent<Button>();
        cardView = GetComponent<CardView>();

        button.onClick.AddListener(SelectCard);
    }

    private void OnDestroy()
    {
        if (button != null)
            button.onClick.RemoveListener(SelectCard);
    }

    private void SelectCard()
    {
        targetingController.SelectCard(cardView);
    }
}
