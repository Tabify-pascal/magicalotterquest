
public enum EnemyIntentType
{
    Attack,
    Block,
    Buff,
    Debuff,
    Heal,
    Unknown
}

[System.Serializable]
public class EnemyIntent
{
    public EnemyIntentType Type { get; }
    public int Value { get; }

    public EnemyIntent(EnemyIntentType type, int value = 0)
    {
        Type = type;
        Value = value;
    }
}
