using UnityEngine;
using NaughtyAttributes;
using NUnit.Framework;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Store Config", menuName = "Store/Store config")]

public class Store : ScriptableObject
{
    //Variables
    [BoxGroup("Store Variables")]
    public List<BuyableItems> buyableItems = new List<BuyableItems>();

    //Functions
}

