public class CardInstance
{
    public CardData Data { get; }
    public bool IsUpgraded { get; private set; }

    public string Name => Data.cardName + (IsUpgraded ? "+" : "");

    public CardType Type => Data.cardType;

    public CardTargetType TargetType => Data.targetType;

    public int EnergyCost => Data.energyCost;

    public int Damage => IsUpgraded
        ? Data.upgradedDamage
        : Data.damage;

    public int Block => IsUpgraded
        ? Data.upgradedBlock
        : Data.block;

    public CardInstance(CardData data, bool isUpgraded = false)
    {
        Data = data;
        IsUpgraded = isUpgraded;
    }

    public void Upgrade()
    {
        IsUpgraded = true;
    }
}