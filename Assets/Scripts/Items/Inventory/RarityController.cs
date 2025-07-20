using UnityEngine;
using NaughtyAttributes;
using System;
using Unity.VisualScripting;

public class RarityController : MonoBehaviour, I_RarityController
{
    #region Variables

    [Header("Instancia")]
    public static RarityController instance;

    [BoxGroup("Rarity effects")]
    public GameObject[] VFXs = new GameObject[Enum.GetValues(typeof(Rarity)).Length];

    [BoxGroup("Rarity Colors")]
    [SerializeField] private Color commonColor = Color.black;
    [BoxGroup("Rarity Colors")]
    [SerializeField] private Color rareColor = Color.blue;
    [BoxGroup("Rarity Colors")]
    [SerializeField] private Color epicColor = Color.violet;
    [BoxGroup("Rarity Colors")]
    [SerializeField] private Color legendaryColor = Color.yellowNice;

    [Header("Private variables")]
    private int[] rarityAmounts = new int[Enum.GetValues(typeof(Rarity)).Length];

    #endregion

    #region Functions

    private void Awake() => SetStartingAttributes();

    public void SetStartingAttributes()
    {
        if (instance) Destroy(this);
        else instance = this;
    }

    /// <summary>
    /// Crea el VFX para un item
    /// </summary>
    /// <param name="itemPosition"></param>
    /// <param name="rarity"></param>
    /// <returns></returns>
    public GameObject CreateVFX(Transform itemPosition, Rarity rarity) 
    {
        return Instantiate(VFXs[(int)rarity], itemPosition.position, Quaternion.identity, this.transform);
    }

    /// <summary>
    /// Obtiene la cantidad de valor multiplicado por la rareza del item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public int GetRarityAmount(Items item) 
    {
        if (item is Equipment) 
        {
            if (item.value == 0) return 1;

            switch (item.rarity)
            {
                case Rarity.Common:
                    return item.value * rarityAmounts[(int)Rarity.Common];
                case Rarity.Rare:
                    return item.value * rarityAmounts[(int)Rarity.Rare];
                case Rarity.Epic:
                    return item.value * rarityAmounts[(int)Rarity.Epic];
                case Rarity.Legendary:
                    return item.value * rarityAmounts[(int)Rarity.Legendary];
                default:
                    return item.value;
            }
        }

        return 1;
    }

    /// <summary>
    /// Devuelve el color en base a la rareza
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public Color GetRarityColor(Items item) 
    {
        switch (item.rarity)
        {
            case Rarity.Common:
                return commonColor;
            case Rarity.Rare:
                return rareColor;
            case Rarity.Epic:
                return epicColor;
            case Rarity.Legendary:
                return legendaryColor;
            default:
                return commonColor;
        }
    }

    #endregion
}
