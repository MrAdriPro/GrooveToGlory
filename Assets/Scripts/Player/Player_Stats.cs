using UnityEngine;
using NaughtyAttributes;
using System;

[Serializable]
public static class Player_Stats
{
    //Variables

    [Header("Player stats")]
    public static int currentHealth = 10;
    public static int armor = 1;
    public static int strength = 1;
    public static int speed = 1;
    public static int criticChance = 1;

}


[Serializable]
public static class Inventory_Stats
{
    [Header("Inventory Stats")]
    public static int totalConsumablecells = 1;
    public static bool inventoryOpened = false;
}
