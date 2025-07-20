using UnityEngine;
using NaughtyAttributes;
using Unity.VisualScripting;
using System;

[Serializable]
public class Items : ScriptableObject
{
    //Variables
    [Header("Variables")]
    [BoxGroup("Variables")]
    public int itemId = 0;
    [BoxGroup("Variables")]
    public string itemName = string.Empty;
    [BoxGroup("Variables")]
    public string itemDescription = string.Empty;
    [BoxGroup("Variables")]
    public Rarity rarity;
    [BoxGroup("Variables")]
    public int value = 1;
    [BoxGroup("Variables")]
    public Sprite itemIcon;
    [BoxGroup("Variables")]
    public GameObject itemPrefab;

    //Constructor
    public Items(int itemId, string itemName, string itemDescription, Rarity rarity, int value)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.itemDescription = itemDescription;
        this.rarity = rarity;
        this.value = value;
    }
}


public enum Rarity : byte
{
    Common,
    Rare,
    Epic,
    Legendary
}

public enum ItemType : byte
{
    Equipment,
    Consumable,
    Ability,
    Currency,
    Material
}
