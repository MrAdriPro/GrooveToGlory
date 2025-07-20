using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Recipes", menuName = "Recipes/New Recipes")]
public class Recipes : ScriptableObject
{
    public string recipeName = string.Empty;
    public List<ItemCraftInfo> items;
}


public class ItemCraftInfo 
{
    public Items item;
    public int amount;
}
