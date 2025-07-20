using UnityEngine;
using NaughtyAttributes;

public interface I_GenericItemHolder
{

    /// <summary>
    /// Establece las propiedades del objeto
    /// </summary>
    void SetItemSettings();

    /// <summary>
    /// Obtiene el tipo de Item
    /// </summary>
    /// <returns></returns>
    ItemType GetItemType();
}
