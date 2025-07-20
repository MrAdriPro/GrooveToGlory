using UnityEngine;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;
using System.Collections.Generic;

public class ItemPickup : MonoBehaviour, I_ItemPickup
{
    #region Variables

    [Header("Item Status")]
    [BoxGroup("Item Status")]
    private bool canBePickedUp = false;
    [BoxGroup("Item Status")]
    private GenericItemHolder item = null;

    [Header("Pickup Variables")]
    [SerializeField] private float minDistance = 10f;
    [SerializeField] private LayerMask layers;
    [SerializeField] private float outlineThickness;
    [SerializeField] private Color outlineColor;



    private new Renderer renderer;
    private Camera playerCam;
    private Transform playerBody;
    private Inventory inventory;
    private GenericItemHolder gih;
    private ChestBehaviour actualChest = null;

    #endregion
    //Functions

    private void Start() => SetStartingAttributes();

    private void Update() => InputsController();


    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes()
    {
        playerCam = GameObject.FindGameObjectWithTag(Tags.CAMERA_TAG).GetComponent<Camera>();
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Transform>();
        inventory = GetComponent<Inventory>();
    }

    /// <summary>
    /// Controlador de los inputs
    /// </summary>
    public void InputsController() 
    {
        OnItemHover();

        if (item && canBePickedUp && !Extensions.Chest_Indicator)
            Extensions.Item_Indicator = true;

        else Extensions.Item_Indicator = false;

        if (Input.GetKeyDown(Player_Inputs.LeftClick)) PickupItem();
    }

    /// <summary>
    /// Un metodo demasiado largo con el raycast para comprobar la posicion del raton y permitir interactuar con los objetos
    /// </summary>
    public void OnItemHover()
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000, layers))
        {
            if (Vector3.Distance(playerBody.position, hit.collider.transform.position) < minDistance)
            {
                switch (hit.collider.tag)
                {
                    case Tags.ITEM_TAG:
                        renderer = hit.collider.GetComponent<Renderer>();
                        if (actualChest)
                        {
                            actualChest.canBeInteracted = false;
                            Extensions.Chest_Indicator = false;
                        }
                        renderer.material.SetFloat("_OutlineThickness", outlineThickness);
                        gih = hit.collider.GetComponent<GenericItemHolder>();
                        if (gih != null) SetItemStatus(gih);
                        break;
                    case Tags.CHEST_TXT:
                        renderer = hit.collider.GetComponentInChildren<Renderer>();
                        actualChest = hit.collider.GetComponent<ChestBehaviour>();
                        if (!actualChest.opened)
                        {
                            renderer.material.SetFloat("_OutlineThickness", outlineThickness);
                            actualChest.canBeInteracted = true;
                            SetItemStatus(false);
                        }
                        break;
                    default:
                        Hud_Controller.Instance.statsMenuController.ActivateInfoCompareHud(null, null);
                        SetObjectStatusToFalse();
                        break;
                }

            }
            else
            {
                Hud_Controller.Instance.statsMenuController.ActivateInfoCompareHud(null, null);
                SetObjectStatusToFalse();
            }
        }
    }

    /// <summary>
    /// Metodo que pone los estados necesarios a falso
    /// </summary>
    public void SetObjectStatusToFalse() 
    {
        if (renderer != null) renderer.material.SetFloat("_OutlineThickness", 0);
        if (actualChest)
        {
            actualChest.canBeInteracted = false;
            Extensions.Chest_Indicator = false;
        }
        SetItemStatus(false);
    }

    /// <summary>
    /// Obtiene el objeto
    /// </summary>
    public void PickupItem() 
    {
        if (!item || !canBePickedUp) return;
        GetItem();
    }

    /// <summary>
    /// Sets the item status
    /// </summary>
    /// <param name="_object"></param>
    public void SetItemStatus(GenericItemHolder item) 
    {
        canBePickedUp = true;
        this.item = item;
    }
    /// <summary>
    /// Establece el status del item
    /// </summary>
    /// <param name="status"></param>
    public void SetItemStatus(bool status) 
    {
        canBePickedUp = status;
        item = null;
    }

    /// <summary>
    /// Obtiene la informacion del objeto
    /// </summary>
    public void GetItem() 
    {
        bool objectPicked = false;

        try
        {
            bool canBuyItem = inventory.CanBuyItem(item.currencyToUse, item.buyAmount);
            if (item.isBuyable && canBuyItem)
            {
                switch (item.GetItemType())
                {
                    case ItemType.Equipment:

                        objectPicked = inventory.AddEquipment((Equipment)item.item);
                        ObjectBuyed(objectPicked);
                        break;
                    case ItemType.Consumable:
                        objectPicked = inventory.AddConsumables((Consumable)item.item);
                        ObjectBuyed(objectPicked);
                        break;
                    case ItemType.Ability:
                        break;
                    case ItemType.Currency:
                        break;
                    case ItemType.Material:
                        List<Material> list = new List<Material>();
                        for (int i = 0; i < item.materialAmount; i++)
                        {
                            list.Add((Material)item.item);
                        }
                        objectPicked = inventory.AddMaterial(list);
                        ObjectBuyed(objectPicked);
                        break;
                    default:
                        break;
                }
            }
            else if (!item.isBuyable) 
            {
                switch (item.GetItemType())
                {
                    case ItemType.Equipment:
                        objectPicked = inventory.AddEquipment((Equipment)item.item);
                        ObjectPicked(objectPicked);
                        break;
                    case ItemType.Consumable:
                        objectPicked = inventory.AddConsumables((Consumable)item.item);
                        ObjectPicked(objectPicked);
                        break;
                    case ItemType.Ability:
                        break;
                    case ItemType.Currency:
                        objectPicked = inventory.AddCurrency((Currency)item.item, item.currencyAmount);
                        InfoText("+ " + item.currencyAmount + " " + item.item.itemName, Color.yellow);
                        break;
                    case ItemType.Material:
                        List<Material> list = new List<Material>();
                        for (int i = 0; i < item.materialAmount; i++)
                        {
                            list.Add((Material)item.item);
                        }
                        objectPicked = inventory.AddMaterial(list);
                        ObjectPicked(objectPicked);
                        break;
                    default:
                        break;
                }

            }
        }
        catch (Exception) { }

        if (!objectPicked) return;

        DeleteItemFromWorld();
    }

    /// <summary>
    /// Destruye el item del mundo
    /// </summary>
    public void DeleteItemFromWorld() 
    {
        Destroy(item.GetComponent<ItemDrop>().VFX.gameObject);
        Destroy(item.gameObject);
        SetItemStatus(false);
    }

    /// <summary>
    /// Metodo que establece que el objeto se ha comprado
    /// </summary>
    /// <param name="objectPicked"></param>
    public void ObjectBuyed(bool objectPicked) 
    {

        if (objectPicked)
        {
            InfoText("Picked up " + item.item.itemName, RarityController.instance.GetRarityColor(item.item));
            inventory.RemoveCurrency(item.currencyToUse, item.buyAmount);
            InfoText("- " + item.buyAmount + " " + item.currencyToUse.itemName, Color.red, 1);

        }
        else InfoText("Failed to pick up " + item.item.itemName, Color.red);
    }

    /// <summary>
    /// Metodo que establece que el objeto se ha recogido
    /// </summary>
    /// <param name="objectPicked"></param>
    public void ObjectPicked(bool objectPicked) 
    {
        if (objectPicked)
            InfoText("Picked up " + item.item.itemName, RarityController.instance.GetRarityColor(item.item));
        else
            InfoText("Failed to pick up " + item.item.itemName, Color.red);
    }


    /// <summary>
    /// Spawnea el texto de info
    /// </summary>
    /// <param name="message"></param>
    public void InfoText(string message, Color color, float time) 
    {
        StartCoroutine(Text_Spawner.instance.SpawnFloatingText(playerBody.position,
                                        message,
                                        color, time));
    }

    /// <summary>
    /// Spawnea el texto de info
    /// </summary>
    /// <param name="message"></param>
    public void InfoText(string message, Color color)
    {
        StartCoroutine(Text_Spawner.instance.SpawnFloatingText(playerBody.position,
                                        message,
                                        color, 0));
    }

}
