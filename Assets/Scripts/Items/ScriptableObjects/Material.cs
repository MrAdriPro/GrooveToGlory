using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "New Material",menuName = "Items/Material")]
public class Material : Items
{
    //Variables
    //Functions
    public Material(int itemId, string itemName, string itemDescription, Rarity rarity, int value) : base(itemId, itemName, itemDescription, rarity, value)
    {
    }
}
