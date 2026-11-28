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
    public CardType cardType = CardType.Attack;
    public CardTargetType targetType = CardTargetType.Enemy;

    [Header("Energy")]
    [Min(0)] public int energyCost = 1;

    [Header("Damage")]
    [Min(0)] public int damage;
    [Min(0)] public int upgradedDamage;

    [Header("Block")]
    [Min(0)] public int block;
    [Min(0)] public int upgradedBlock;

    [Header("Description")]
    [TextArea]
    public string description;
}