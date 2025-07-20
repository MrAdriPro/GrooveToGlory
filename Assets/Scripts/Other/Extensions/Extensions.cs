using UnityEngine;
using NaughtyAttributes;
using static UnityEditor.Progress;

public static class Extensions
{
    //Variables
    public static bool Chest_Indicator = false;
    public static bool Item_Indicator = false;

    public static string ARMOR_TXT = "Armor ";
    public static string STRENGTH_TXT = "Strength ";
    public static string SPEED_TXT = "Speed ";
    public static string CRITCHANGE_TXT = "Crit ";

    public static bool isPauseMenuOpened = false;

    public static GameObject slotChecking = null;
    public static bool useMenuActive = false;

    
    public static float DistanceSquared(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) +
               (a.y - b.y) * (a.y - b.y) +
               (a.z - b.z) * (a.z - b.z);
    }


}
