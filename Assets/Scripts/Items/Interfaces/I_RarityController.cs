using UnityEngine;
using NaughtyAttributes;

public interface I_RarityController : I_GenericMethods
{

    /// <summary>
    /// Crea el VFX para un item
    /// </summary>
    /// <param name="itemPosition"></param>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public GameObject CreateVFX(Transform itemPosition, Rarity rarity);
    /// <summary>
    /// Obtiene la cantidad de valor multiplicado por la rareza del item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetRarityAmount(Items item);
    /// <summary>
    /// Devuelve el color en base a la rareza
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Color GetRarityColor(Items item);
}
