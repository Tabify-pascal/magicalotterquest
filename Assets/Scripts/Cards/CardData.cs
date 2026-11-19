using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCard",
    menuName = "MagicalOtterQuest/Card"
)]
public class CardData : ScriptableObject
{
    [Header("Identity")]
    public string cardName;
    public Sprite artwork;

    [Header("Gameplay")]
    [Min(0)] public int energyCost = 1;
    [Min(0)] public int damage = 1;
    [Min(0)] public int upgradedDamage = 2;

    [Header("Description")]
    [TextArea]
    public string description = "Deal 1 damage.";
}
