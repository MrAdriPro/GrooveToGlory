using UnityEngine;
using NaughtyAttributes;

public interface I_StatsMenu
{
    //Variables

    /// <summary>
    /// Establece lo necesario para comparar los stats
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="equipedEquipment"></param>
    public void SetCompareStats(Equipment newEquipment, Equipment equipedEquipment);
    /// <summary>
    /// Compara los stats
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="equipedEquipment"></param>
    public void CompareStat(Equipment newEquipment, Equipment equipedEquipment);
    /// <summary>
    /// Pone los sprites en mejor
    /// </summary>
    public void SetEquipedItemStats();
    /// <summary>
    /// Pone los sprites en mejor
    /// </summary>
    public void SetNewItemStats();
}
