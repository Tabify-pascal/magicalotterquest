
using System.Collections.Generic;
using UnityEngine;

public class EnemyIntentContainer : MonoBehaviour
{
    [SerializeField] private EnemyIntentUI intentPrefab;
    [SerializeField] private Transform iconContainer;

    private readonly List<EnemyIntentUI> activeIcons = new();

    public void ShowIntents(IEnumerable<EnemyIntent> intents)
    {
        Clear();

        foreach (EnemyIntent intent in intents)
        {
            EnemyIntentUI icon = Instantiate(
                intentPrefab,
                iconContainer
            );

            icon.ShowIntent(intent);
            activeIcons.Add(icon);
        }
    }

    public void Clear()
    {
        foreach (EnemyIntentUI icon in activeIcons)
        {
            if (icon != null)
                Destroy(icon.gameObject);
        }

        activeIcons.Clear();
    }
}
