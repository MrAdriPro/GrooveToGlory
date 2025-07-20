using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public interface I_Inventory : I_GenericMethods
{

    #region Equipments
    /// <summary>
    /// Cambia el equipamiento
    /// </summary>
    /// <param name="_equipment"></param>
    /// <returns></returns>
    public bool SetEquipment(Equipment _equipment, bool dropItem, int slotPos);

    /// <summary>
    /// Añade equipamiento al inventario
    /// </summary>
    /// <param name="equipment"></param>
    public bool AddEquipment(Equipment equipment);

    /// <summary>
    /// Quita del inventario un equipamiento
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool RemoveEquipment(Equipment equipment, int slotPos);

    /// <summary>
    /// Actualiza los stats para un intercambio de equipamiento
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="position"></param>
    /// <param name="dropItem"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool UpdateStats(Equipment newEquipment, int position, bool dropItem, int slotPos);

    /// <summary>
    /// Actualiza los stats para un item nuevo
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool UpdateStats(Equipment item, int slotPos);

    /// <summary>
    /// Actualiza los stats del personaje dependiendo de la cantidad y el objetivo
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateStats(int[] amount);

    /// <summary>
    /// Obtiene el equipamiento por la posicion pasada
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Equipment GetEquipmentByPosition(int position);

    #endregion

    #region Consumibles

    /// <summary>
    /// Añade el consumible al inventario
    /// </summary>
    /// <param name="consumable"></param>
    /// <returns></returns>
    public bool AddConsumables(Consumable consumable);

    /// <summary>
    /// Llama al Consumable_Behaviour para utilizar el objeto consumible
    /// </summary>
    /// <param name="consumablePosition"></param>
    public void UseConsumable(int consumablePosition);
    

    #endregion

    #region Currencies

    /// <summary>
    /// Añade una cantidad de monedas al inventario
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public bool AddCurrency(Currency currency, int amount);

    /// <summary>
    /// Elimina una cantidad de monedas del inventario
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public bool RemoveCurrency(Currency currency, int amount);

    /// <summary>
    /// Comprueba si se puede comprar el objeto
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public bool CanBuyItem(Currency currency, int amount);
    #endregion

    #region Materials

    /// <summary>
    /// Añade materiales al inventario
    /// </summary>
    /// <param name="newMaterials"></param>
    public bool AddMaterial(List<Material> newMaterials);

    /// <summary>
    /// Quita del inventario una cantidad especifica de un material
    /// </summary>
    /// <param name="newMaterials"></param>
    /// <returns></returns>
    public bool RemoveMaterial(List<Material> newMaterials, int slotPos, bool dropItem);

    #endregion

    #region Create Items
    /// <summary>
    /// Metodo que crea un item en el mundo con su configuracion necesaria
    /// </summary>
    /// <param name="item"></param>
    public void CreateItem(Items item, int? amount);


    /// <summary>
    /// Establece las propiedades del scrip ItemDrop
    /// </summary>
    /// <param name="itemDropSettings"></param>
    /// <param name="item"></param>
    public void SetItemDropSettings(ItemDrop itemDropSettings, Items item);

    #endregion

    #region Slots

    /// <summary>
    /// Inicializa los slots de los materiales
    /// </summary>
    /// <param name="newSlots"></param>
    public void SetMaterialSlots(List<Slot_Behaviour> newSlots);

    /// <summary>
    /// Inicializa los slots de los equipamientos
    /// </summary>
    /// <param name="newSlots"></param>
    public void SetEquipmentSlots(List<Slot_Behaviour> newSlots);
    #endregion
}
