using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Currency", menuName = "Currency/New Currency")]
public class Currency : Items
{
    //Variables
    public Currency(int itemId, string itemName, string itemDescription, Rarity rarity, int value) : base(itemId, itemName, itemDescription, rarity, value)
    {
    }
}

public enum CurrencyType 
{
    GoldCoin,
    SoulCoin
}
