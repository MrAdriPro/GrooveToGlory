using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "New Chest", menuName = "Items/Chest", order = 1)]
public class Chest : ScriptableObject
{
    //Variables
    [Space(20)]
    [BoxGroup("Chest Attributes")]
    public int chestCapacity;
    [BoxGroup("Chest Attributes")]
    public Rarity rarity;
    [BoxGroup("Chest Attributes")]
    public Sprite chestSprite;
    [BoxGroup("Chest Attributes")]
    public float chestOpenTime = 1;
    [Header("Chest Items")]
    [Space(10)]
    [Label("Importante: Cada item tiene que estar en la misma posicion \n (Prefab,Info y Porcentaje)")]
    [Space(10)]
    public ChestValues chestValues;
    
}

[Serializable]
public class ChestValues 
{
    public Items[] items;
    public float[] percentage;

    public Currency[] currencies;
    public int[] maxAmount;
    public int[] currencyPercentage;
}


