using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Buyable Item", menuName = "Items/Buyable Item")]
public class BuyableItems : ScriptableObject
{
    //Variables

    [BoxGroup("Buyable items configuration")]
    public Items item = null;
    [BoxGroup("Buyable items configuration")]
    public Currency currencyUsed;
    [BoxGroup("Buyable items configuration")]
    public int itemPrice = 1;
    

    //Functions
}
