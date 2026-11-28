using System;
using UnityEngine;

[Serializable]
public class StartingDeckEntry
{
    public CardData card;

    [Min(1)]
    public int amount = 1;

    public bool isUpgraded;
}