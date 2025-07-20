using UnityEngine;
using NaughtyAttributes;
using System;
using System.Collections.Generic;

public class Inventory : MonoBehaviour , I_Inventory
{
    #region Variables
    [BoxGroup("Items lists")]
    [SerializeField] private Equipment[] equipment = new Equipment[Enum.GetValues(typeof(Placeholder)).Length];
    [BoxGroup("Items lists")]
    [SerializeField] private Consumable[] consumables = new Consumable[Inventory_Stats.totalConsumablecells];
    [BoxGroup("Items lists")]
    [SerializeField] private List<Material> materials = new List<Material>();
    [BoxGroup("Items lists")]
    [SerializeField] private Currency_Inventory[] currencies = new Currency_Inventory[Enum.GetValues(typeof(CurrencyType)).Length];

    [BoxGroup("Slots lists")]
    [SerializeField] private List<Slot_Behaviour> materialSlots = new List<Slot_Behaviour>();
    [BoxGroup("Slots lists")]
    [SerializeField] private List<Slot_Behaviour> equipmentSlots = new List<Slot_Behaviour>();
    [Header("Private Variables")]
    private GameObject playerBody = null;

    #endregion

    private void Start() => SetStartingAttributes();

    private void Update() => InputsController();
    
    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes() 
    {
        Hud_Controller.Instance.SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);
        Hud_Controller.Instance.UpdateItemIcons(equipment);
        Hud_Controller.Instance.UpdateGoldCoinsText(currencies[(int)CurrencyType.GoldCoin].amount);
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG);
    }

    /// <summary>
    /// Controlador de inputs
    /// </summary>
    public void InputsController() 
    {
        if (Input.GetKeyDown(Player_Inputs.Consumable1) && consumables[0]) UseConsumable(1);

        if (Inventory_Stats.totalConsumablecells >= 2)
            if (Input.GetKeyDown(Player_Inputs.Consumable2) && consumables[1] != null) UseConsumable(2);
    }

    #region Equipment

    /// <summary>
    /// Cambia el equipamiento
    /// </summary>
    /// <param name="_equipment"></param>
    /// <returns></returns>
    public bool SetEquipment(Equipment _equipment, bool dropItem, int slotPos)
    {
        if (equipment[(int)_equipment.placeholder] != null)
        {
            UpdateStats(_equipment, (int)_equipment.placeholder, dropItem, slotPos);
        }
        else
        {
            equipment[(int)_equipment.placeholder] = _equipment;
            UpdateStats(_equipment, slotPos);
        }

        return true;
    }

    /// <summary>
    /// Añade equipamiento al inventario
    /// </summary>
    /// <param name="equipment"></param>
    public bool AddEquipment(Equipment equipment)
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            if (equipmentSlots[i].IsEmpty)
            {
                equipmentSlots[i].itemsInSlot.Add(equipment);
                equipmentSlots[i].SetItemIcon(true, equipment.itemIcon);
                equipmentSlots[i].SetItemAmountText(1);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Quita del inventario un equipamiento
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool RemoveEquipment(Equipment equipment, int slotPos)
    {
        for (int i = 0; i < equipmentSlots.Count; i++)
        {
            if (!equipmentSlots[i].IsEmpty)
            {
                if (equipmentSlots[i].GetItemInSlot.Equals(equipment) && i == slotPos)
                {
                    equipmentSlots[i].itemsInSlot.Clear();
                    equipmentSlots[i].SetItemIcon(false, null);
                    return true;

                }
            }
        }

        return false;
    }

    /// <summary>
    /// Actualiza los stats para un intercambio de equipamiento
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="position"></param>
    /// <param name="dropItem"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool UpdateStats(Equipment newEquipment, int position, bool dropItem, int slotPos)
    {
        Player_Stats.armor -= equipment[position].armor * RarityController.instance.GetRarityAmount(equipment[position]);
        Player_Stats.strength -= equipment[position].strength * RarityController.instance.GetRarityAmount(equipment[position]);
        Player_Stats.speed -= equipment[position].speed * RarityController.instance.GetRarityAmount(equipment[position]);
        Player_Stats.criticChance -= equipment[position].criticChance * RarityController.instance.GetRarityAmount(equipment[position]);

        if (dropItem) CreateItem(equipment[position], null);
        RemoveEquipment(newEquipment, slotPos);
        AddEquipment(equipment[position]);
        equipment[position] = newEquipment;

        Player_Stats.armor += newEquipment.armor * RarityController.instance.GetRarityAmount(newEquipment);
        Player_Stats.strength += newEquipment.strength * RarityController.instance.GetRarityAmount(newEquipment);
        Player_Stats.speed += newEquipment.speed * RarityController.instance.GetRarityAmount(newEquipment);
        Player_Stats.criticChance += newEquipment.criticChance * RarityController.instance.GetRarityAmount(newEquipment);

        Hud_Controller.Instance.UpdateItemIcons(equipment);

        return false;
    }

    /// <summary>
    /// Actualiza los stats para un item nuevo
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slotPos"></param>
    /// <returns></returns>
    public bool UpdateStats(Equipment item, int slotPos)
    {

        // Primero comprueba el equipamiento

        Player_Stats.armor += item.armor * RarityController.instance.GetRarityAmount(item);
        Player_Stats.strength += item.strength * RarityController.instance.GetRarityAmount(item);
        Player_Stats.speed += item.speed * RarityController.instance.GetRarityAmount(item);
        Player_Stats.criticChance += item.criticChance * RarityController.instance.GetRarityAmount(item);

        Hud_Controller.Instance.SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);

        Hud_Controller.Instance.UpdateItemIcons(equipment);

        RemoveEquipment(item, slotPos);

        return true;
    }

    /// <summary>
    /// Actualiza los stats del personaje dependiendo de la cantidad y el objetivo
    /// </summary>
    /// <param name="amount"></param>
    public void UpdateStats(int[] amount)
    {
        Player_Stats.armor += amount[(int)ConsumableUses.AddsArmor];
        Player_Stats.strength += amount[(int)ConsumableUses.AddStrenth];
        Player_Stats.speed += amount[(int)ConsumableUses.AddSpeed];
        Player_Stats.criticChance += amount[(int)ConsumableUses.AddCritChance];
        Player_Stats.currentHealth += amount[(int)ConsumableUses.RestoresHealth];

        Hud_Controller.Instance.SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);

    }

    /// <summary>
    /// Obtiene el equipamiento por la posicion pasada
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Equipment GetEquipmentByPosition(int position) => equipment[position];

    #endregion

    #region Consumibles

    /// <summary>
    /// Añade el consumible al inventario
    /// </summary>
    /// <param name="consumable"></param>
    /// <returns></returns>
    public bool AddConsumables(Consumable consumable) 
    {
        for (int i = 0; i < consumables.Length; i++)
        {
            if (consumables[i] == null) 
            {
                consumables[i] = consumable;
                Hud_Controller.Instance.UpdateItemIcons(consumables);
                return true;
            }

		}

        Hud_Controller.Instance.UpdateItemIcons(consumables);

        return false;

    }

    /// <summary>
    /// Llama al Consumable_Behaviour para utilizar el objeto consumible
    /// </summary>
    /// <param name="consumablePosition"></param>
    public void UseConsumable(int consumablePosition)
    {
        GetComponent<ConsumableBehaviour>().UseConsumable(consumables[consumablePosition - 1]);
        consumables[consumablePosition - 1] = null;
        Hud_Controller.Instance.UpdateItemIcons(consumables);

    }

    #endregion

    #region Currencies

    /// <summary>
    /// Añade una cantidad de monedas al inventario
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public bool AddCurrency(Currency currency, int amount) 
    {
        for (int i = 0; i < currencies.Length; i++)
        {
            if (currencies[i].currency.Equals(currency))
            {
                currencies[i].amount += amount;
                Hud_Controller.Instance.UpdateGoldCoinsText(currencies[i].amount);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Elimina una cantidad de monedas del inventario
    /// </summary>
    /// <param name="currency"></param>
    /// <param name="amount"></param>
    public bool RemoveCurrency(Currency currency, int amount)
    {
        for (int i = 0; i < currencies.Length; i++)
        {
            if (currencies[i].currency.Equals(currency))
            {
                currencies[i].amount -= amount;
                Hud_Controller.Instance.UpdateGoldCoinsText(currencies[i].amount);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Comprueba si se puede comprar el objeto
    /// </summary>
    /// <param name="currency"></param>
    /// <returns></returns>
    public bool CanBuyItem(Currency currency, int amount) 
    {
        for (int i = 0; i < currencies.Length; i++)
        {
            if (currencies[i].currency != null)
            {
                if (currencies[i].currency.Equals(currency))
                {
                    if (currencies[i].amount - amount >= 0)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
    #endregion

    #region Materials

    /// <summary>
    /// Añade materiales al inventario
    /// </summary>
    /// <param name="newMaterials"></param>
    public bool AddMaterial(List<Material> newMaterials) 
    {
        for (int i = 0; i < materialSlots.Count; i++)
        {
            if (materialSlots[i].IsEmpty)
            {
                foreach (var material in newMaterials)
                {
                    materialSlots[i].itemsInSlot.Add((Items)material);
                    materials.Add(material);
                }

                materialSlots[i].SetItemIcon(true,newMaterials[0].itemIcon);
                materialSlots[i].SetItemAmountText(materialSlots[i].itemsInSlot.Count);
                return true;
            }
            else if (materialSlots[i].GetItemInSlot.Equals(newMaterials[0])) 
            {
                foreach (var material in newMaterials)
                {
                    materials.Add(material);
                    materialSlots[i].itemsInSlot.Add(material);
                }
                materialSlots[i].SetItemIcon(true, newMaterials[0].itemIcon);
                materialSlots[i].SetItemAmountText(materialSlots[i].itemsInSlot.Count);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Quita del inventario una cantidad especifica de un material
    /// </summary>
    /// <param name="newMaterials"></param>
    /// <returns></returns>
    public bool RemoveMaterial(List<Material> newMaterials, int slotPos, bool dropItem) 
    {
        for (int i = 0; i < materialSlots.Count; i++)
        {
            if (!materialSlots[i].IsEmpty) 
            {
                if (materialSlots[i].GetItemInSlot.Equals(newMaterials[0]) && i == slotPos) 
                {
                    if (materialSlots[i].itemsInSlot.Count >= newMaterials.Count)
                    {
                        if (dropItem) CreateItem(newMaterials[0], newMaterials.Count);
                        foreach (var material in newMaterials)
                        {
                            materialSlots[i].itemsInSlot.Remove(material);
                            materials.Remove(material);
                        }
                        materialSlots[i].SetItemIcon(false, null);
                        materialSlots[i].SetItemAmountText(0);
                        return true;
                    }
                    else
                    {
                        if (dropItem) CreateItem(newMaterials[0], newMaterials.Count);
                        materialSlots[i].itemsInSlot.Clear();
                        materials.RemoveAll(mat => mat.Equals(newMaterials[0]));
                        materialSlots[i].SetItemIcon(false, null);
                        materialSlots[i].SetItemAmountText(0);
                        return true;
                    }

                }
            }
        }

        return false;
    }
    #endregion

    #region Create Items
    /// <summary>
    /// Metodo que crea un item en el mundo con su configuracion necesaria
    /// </summary>
    /// <param name="item"></param>
    public void CreateItem(Items item, int? amount)
    {
        //Crea el objeto y recoge su informacion
        GameObject instantiatedObject = instantiatedObject = Instantiate(item.itemPrefab, playerBody.transform.position, Quaternion.identity);
        SetItemDropSettings(instantiatedObject.GetComponent<ItemDrop>(), item);
        GenericItemHolder gih = instantiatedObject.GetComponent<GenericItemHolder>();

        //Si existe por algun casual GenericItemHolder lo elimina, ya que establecemos uno nosotros
        if (gih != null) Destroy(gih);

        //Establecemos la configuracion del GenericItemHolder
        gih = instantiatedObject.AddComponent<GenericItemHolder>();
        gih.item = item;
        if(item is Material) gih.materialAmount = amount == null ? 0 : (int)amount;

    }


    /// <summary>
    /// Establece las propiedades del scrip ItemDrop
    /// </summary>
    /// <param name="itemDropSettings"></param>
    /// <param name="item"></param>
    public void SetItemDropSettings(ItemDrop itemDropSettings, Items item)
    {
        Physics.IgnoreCollision(itemDropSettings.itemCollider, playerBody.GetComponent<Collider>(), true);
        itemDropSettings.startPos = playerBody.transform;
        itemDropSettings.SetStartingDropSettings(item);
    }
    #endregion

    #region Slots

    /// <summary>
    /// Inicializa los slots de los materiales
    /// </summary>
    /// <param name="newSlots"></param>
    public void SetMaterialSlots(List<Slot_Behaviour> newSlots)
    {
        for (int i = 0; i < newSlots.Count; i++)
        {
            newSlots[i].slotPos = i;
            materialSlots.Add(newSlots[i]);
        }

        materialSlots = newSlots;
    }

    /// <summary>
    /// Inicializa los slots de los equipamientos
    /// </summary>
    /// <param name="newSlots"></param>
    public void SetEquipmentSlots(List<Slot_Behaviour> newSlots)
    {
        for (int i = 0; i < newSlots.Count; i++)
        {
            newSlots[i].slotPos = i;
            equipmentSlots.Add(newSlots[i]);
        }

        equipmentSlots = newSlots;
    }
    #endregion

}

[Serializable]
public class Currency_Inventory 
{
    public Currency currency;
    public int amount;
}