using UnityEngine;
using NaughtyAttributes;
using System;
[Serializable]
public class Ability : Items
{
    //Variables


    //Functions
    public Ability(int itemId, string itemName, string itemDescription, Rarity rarity, int value) : 
        base(itemId, itemName, itemDescription, rarity, value)
    {
    }
}
