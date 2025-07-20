using UnityEngine;
using NaughtyAttributes;

public interface I_StoreController : I_GenericMethods
{
    /// <summary>
    /// Crea los items de la tienda
    /// </summary>
    public void SpawnStoreItems();
}
