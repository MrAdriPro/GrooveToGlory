using UnityEngine;
using NaughtyAttributes;

public static class Extensions
{
    //Variables
    public static bool E_Button_Chest = false;
    public static bool E_Button_Item = false;

    public static float DistanceSquared(Vector3 a, Vector3 b)
    {
        return (a.x - b.x) * (a.x - b.x) +
               (a.y - b.y) * (a.y - b.y) +
               (a.z - b.z) * (a.z - b.z);
    }

}
