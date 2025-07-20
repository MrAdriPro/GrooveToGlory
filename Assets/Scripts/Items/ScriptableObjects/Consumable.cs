using UnityEngine;
using NaughtyAttributes;
using System;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Items/Consumable", order = 1)]
[Serializable]
public class Consumable : Items
{
    //Variables

	[BoxGroup("Consumable Stats")]
	public ConsumableType consumableType;

	[BoxGroup("Consumable Stats")]
	public ConsumableUses consumableUses;

	[BoxGroup("Consumable Stats")]
	[ShowIf("ShowHealthAmount")]
	public int healthToRestore = 0;

	[BoxGroup("Consumable Stats")]
	[ShowIf("ShowArmorAmount")]
	public int armorToAdd = 0;

	[BoxGroup("Consumable Stats")]
	[ShowIf("ShowStrengthAmount")]
	public int strengthToAdd = 0;

	[BoxGroup("Consumable Stats")]
	[ShowIf("ShowSpeedAmount")]
	public int speedToAdd = 0;

	[BoxGroup("Consumable Stats")]
	[ShowIf("ShowCritChanceAmount")]
	public int critChanceToAdd = 0;

	[BoxGroup("Consumable Stats")]
	[EnableIf("consumableType", ConsumableType.Temporal)]
	[Tooltip("El tiempo que dura el efecto.")]
	public int consumableTimeEffect = 0;


#pragma warning disable
	private bool ShowHealthAmount() => consumableType == ConsumableType.Temporal &&
			   (consumableUses == ConsumableUses.RestoresHealth || 
			   consumableUses == ConsumableUses.AddMultiple);
	
	private bool ShowArmorAmount() => consumableUses == ConsumableUses.AddsArmor ||
			   consumableUses == ConsumableUses.AddMultiple;
	

	private bool ShowStrengthAmount() => consumableUses == ConsumableUses.AddStrenth ||
			   consumableUses == ConsumableUses.AddMultiple;
	

	private bool ShowSpeedAmount() => consumableUses == ConsumableUses.AddSpeed ||
			   consumableUses == ConsumableUses.AddMultiple;
	

	private bool ShowCritChanceAmount() => consumableUses == ConsumableUses.AddCritChance ||
			   consumableUses == ConsumableUses.AddMultiple;
	

#pragma warning restore
	//Functions
	public Consumable(int itemId, string itemName, string itemDescription, Rarity rarity, int value) : 
        base(itemId, itemName, itemDescription, rarity, value)
    {
    }
}


public enum ConsumableType 
{
    Temporal,
    Permanent
}

public enum ConsumableUses 
{
    RestoresHealth,
    AddsArmor,
    AddStrenth,
    AddSpeed,
    AddCritChance,
    AddMultiple
}
