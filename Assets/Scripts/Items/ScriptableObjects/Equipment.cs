using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Items/Equipment", order = 1)]
[Serializable]
public class Equipment : Items
{
    //Variables
    [Header("Placeholder")]
    public Placeholder placeholder;

    [BoxGroup("Equipment Stats")]
    public int armor = 0;
    [BoxGroup("Equipment Stats")]
    public int strength = 0;
    [BoxGroup("Equipment Stats")]
    public int speed = 0;
    [BoxGroup("Equipment Stats")]
    public int criticChance = 0;

    //Functions
    public Equipment(int itemId, string itemName, string itemDescription, Rarity rarity, int value) : 
        base(itemId, itemName, itemDescription, rarity, value)
    {
    }
}

public enum Placeholder 
{
    Head,
    Chest,
    Ring,
    Legs,
    Bots,
    Weapon
}
