
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyIntentUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image intentIcon;
    [SerializeField] private TMP_Text intentValue;

    [Header("Sprites")]
    [SerializeField] private Sprite attackSprite;
    [SerializeField] private Sprite blockSprite;
    [SerializeField] private Sprite buffSprite;
    [SerializeField] private Sprite debuffSprite;
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite unknownSprite;

    public void ShowIntent(EnemyIntent intent)
    {
        intentIcon.sprite = GetSprite(intent.Type);
        intentIcon.enabled = intentIcon.sprite != null;

        intentValue.text = intent.Value > 0
            ? intent.Value.ToString()
            : "";
    }

    public void HideIntent()
    {
        intentIcon.enabled = false;
        intentValue.text = "";
    }

    private Sprite GetSprite(EnemyIntentType type)
    {
        return type switch
        {
            EnemyIntentType.Attack => attackSprite,
            EnemyIntentType.Block => blockSprite,
            EnemyIntentType.Buff => buffSprite,
            EnemyIntentType.Debuff => debuffSprite,
            EnemyIntentType.Heal => healSprite,
            _ => unknownSprite
        };
    }
}
