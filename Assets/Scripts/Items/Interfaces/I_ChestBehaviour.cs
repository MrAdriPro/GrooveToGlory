using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public interface I_ChestBehaviour
{



    /// <summary>
    /// Controlador de los inputs
    /// </summary>
    void InputController();

    /// <summary>
    /// Metodo que abre el cofre
    /// </summary>
    void OpenChest();

    /// <summary>
    /// Metodo que instancia de manera aleatoria los items genericos al abrir el cofre
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnItems();

    /// <summary>
    /// Metodo que crea un item en el mundo con su configuracion necesaria
    /// </summary>
    /// <param name="item"></param>
    void CreateItem(Items item, int amount);

    /// <summary>
    /// Metodo que establece las variables del ItemDrop de nuestro Item
    /// </summary>
    /// <param name="itemDropSettings"></param>
    /// <param name="item"></param>
    void SetItemDropSettings(ItemDrop itemDropSettings, Items item);
}
