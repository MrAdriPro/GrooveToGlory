using System.Collections.Generic;
using UnityEngine;

public static class TimeExtensions
{
    public static bool IsTimeInList(List<float> list, float currentTime, float tolerance = 0.05f)
    {
        foreach (float t in list)
        {
            if (Mathf.Abs(currentTime - t) <= tolerance) return true;
        }
        return false;
    }

}
