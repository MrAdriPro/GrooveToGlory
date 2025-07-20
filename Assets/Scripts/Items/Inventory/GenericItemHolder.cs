using UnityEngine;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class GenericItemHolder : MonoBehaviour, I_GenericMethods, I_GenericItemHolder
{
    #region Variables

    [BoxGroup("Item stats")]
    public Items item;
    [BoxGroup("Item stats")]
    private ItemPickup itemPickup;

    [BoxGroup("Item stats")]
    [ShowIf("isMaterial")]
    public int materialAmount = 1;

    [BoxGroup("Item stats")]
    [ShowIf("isCurrency")]
    public int currencyAmount;
    [BoxGroup("Item cost")]
    [ShowIf("isCurrency")]
    public Currency currencyToUse;
    [BoxGroup("Item cost")]
    public int buyAmount = 0;

    [Header("Private variables")]
    private TextMeshPro itemText;
    private SpriteRenderer currencyImage;


    #pragma warning disable
    private bool isCurrency => (item is Currency);

    private bool isMaterial => (item is Material);

    public bool isBuyable => buyAmount > 0;

    #pragma warning restore

    #endregion

    #region Functions
    //Functions
    private void Start()
    {
        SetStartingAttributes();
    }

    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes()
    {
        itemPickup = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<ItemPickup>();

        if (!isBuyable) return;

        SetItemSettings();
        itemText.text = $"{item.itemName} {buyAmount}";
        currencyImage.sprite = currencyToUse.itemIcon;
        
    }


    /// <summary>
    /// Establece las propiedades del objeto
    /// </summary>
    public void SetItemSettings() 
    {
        Transform directChild = transform.GetChild(0);
        directChild.gameObject.SetActive(true);
        itemText = GetComponentInChildren<TextMeshPro>();
        currencyImage = directChild.GetComponentInChildren<SpriteRenderer>();

    }

    /// <summary>
    /// Obtiene el tipo de Item
    /// </summary>
    /// <returns></returns>
    public ItemType GetItemType() 
    {
        if (item is Equipment) return ItemType.Equipment;
        if (item is Ability) return ItemType.Ability;
        if (item is Consumable) return ItemType.Consumable;
        if (item is Currency) return ItemType.Currency;
        if (item is Material) return ItemType.Material;
        return ItemType.Equipment;
    }

    #endregion
}
