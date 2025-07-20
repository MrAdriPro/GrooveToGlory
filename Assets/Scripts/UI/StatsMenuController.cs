using UnityEngine;
using NaughtyAttributes;

public class StatsMenuController : MonoBehaviour, I_StatsMenu
{
    #region Set Item Stats

    /// <summary>
    /// Activa o desactiva el HUD de comparacion
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="equipedItem"></param>
    public void ActivateInfoCompareHud(Equipment newItem, Equipment equipedItem) => SetCompareStats(newItem, equipedItem);

    /// <summary>
    /// Activa o desactiva el HUD de comparacion y lo pone en la ubicacion necesaria
    /// </summary>
    /// <param name="newItem"></param>
    /// <param name="equipedItem"></param>
    /// <param name="rt"></param>
    public void ActivateInfoCompareHud(Equipment newItem, Equipment equipedItem, RectTransform rt)
    {

        // Obtener la posición local del slot respecto a su padre (Canvas o contenedor común)
        Vector3 slotWorldPos = rt.position;
        // Calcular la nueva posición con offset hacia la izquierda
        Vector3 nuevaPosWorld = slotWorldPos + new Vector3(-rt.sizeDelta.x - Hud_Controller.Instance.comparePanelOffset.x, Hud_Controller.Instance.comparePanelOffset.y, 0);

        Hud_Controller.Instance.comparePanel.GetComponent<RectTransform>().position = nuevaPosWorld;
        SetCompareStats(newItem, equipedItem);
    }

    /// <summary>
    /// Establece lo necesario para comparar los stats
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="equipedEquipment"></param>
    public void SetCompareStats(Equipment newEquipment, Equipment equipedEquipment)
    {

        Hud_Controller.Instance.comparePanel.SetActive(true);

        if (newEquipment)
        {
            Hud_Controller.Instance.newItemComparePanel.SetActive(true);
            Hud_Controller.Instance.newItemName.text = newEquipment.itemName;
            Hud_Controller.Instance.newItemIcon.sprite = newEquipment.itemIcon;
            Hud_Controller.Instance.newItemArmor.text = $"{Extensions.ARMOR_TXT}  {newEquipment.armor * RarityController.instance.GetRarityAmount(newEquipment)}";
            Hud_Controller.Instance.newItemStrength.text = $"{Extensions.STRENGTH_TXT}  {newEquipment.strength * RarityController.instance.GetRarityAmount(newEquipment)}";
            Hud_Controller.Instance.newItemSpeed.text = $"{Extensions.SPEED_TXT}  {newEquipment.speed * RarityController.instance.GetRarityAmount(newEquipment)}";
            Hud_Controller.Instance.newItemCritChance.text = $"{Extensions.CRITCHANGE_TXT}  {newEquipment.criticChance * RarityController.instance.GetRarityAmount(newEquipment)}";
        }
        else
        {
            Hud_Controller.Instance.newItemComparePanel.SetActive(false);
        }

        if (equipedEquipment)
        {
            Hud_Controller.Instance.equipedItemComparePanel.SetActive(true);
            Hud_Controller.Instance.equipedItemName.text = equipedEquipment.itemName;
            Hud_Controller.Instance.equipedItemIcon.sprite = equipedEquipment.itemIcon;
            Hud_Controller.Instance.equipedItemArmor.text = $"{Extensions.ARMOR_TXT}  {equipedEquipment.armor * RarityController.instance.GetRarityAmount(equipedEquipment)}";
            Hud_Controller.Instance.equipedItemStrength.text = $"{Extensions.STRENGTH_TXT}  {equipedEquipment.strength * RarityController.instance.GetRarityAmount(equipedEquipment)}";
            Hud_Controller.Instance.equipedItemSpeed.text = $"{Extensions.SPEED_TXT}  {equipedEquipment.speed * RarityController.instance.GetRarityAmount(equipedEquipment)}";
            Hud_Controller.Instance.equipedItemCritChance.text = $"{Extensions.CRITCHANGE_TXT}  {equipedEquipment.criticChance * RarityController.instance.GetRarityAmount(equipedEquipment)}";
        }
        else
        {
            Hud_Controller.Instance.equipedItemComparePanel.SetActive(false);
        }

        // Compara los stats y pone cual es mejor o peor
        if (equipedEquipment && newEquipment)
            CompareStat(newEquipment, equipedEquipment);
        else if (newEquipment && !equipedEquipment)
            SetNewItemStats();
        else if (!newEquipment && equipedEquipment)
            SetEquipedItemStats();
        else
        {
            Hud_Controller.Instance.newItemComparePanel.SetActive(false);
            Hud_Controller.Instance.equipedItemComparePanel.SetActive(false);
            Hud_Controller.Instance.comparePanel.SetActive(false);
        }



    }

    /// <summary>
    /// Pone los sprites en mejor
    /// </summary>
    public void SetNewItemStats()
    {
        Hud_Controller.Instance.newItemArmorStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.newItemArmorStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.newItemStrengthStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.newItemStrengthStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.newItemSpeedStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.newItemSpeedStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.newItemCritStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.newItemCritStat.color = Hud_Controller.Instance.betterStatColor;
    }

    /// <summary>
    /// Pone los sprites en mejor
    /// </summary>
    public void SetEquipedItemStats()
    {
        Hud_Controller.Instance.equipedItemArmorStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.equipedItemArmorStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.equipedItemStrengthStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.equipedItemStrengthStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.equipedItemSpeedStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.equipedItemSpeedStat.color = Hud_Controller.Instance.betterStatColor;
        Hud_Controller.Instance.equipedItemCritChanceStat.sprite = Hud_Controller.Instance.betterStatSprite;
        Hud_Controller.Instance.equipedItemCritChanceStat.color = Hud_Controller.Instance.betterStatColor;
    }

    /// <summary>
    /// Compara los stats
    /// </summary>
    /// <param name="newEquipment"></param>
    /// <param name="equipedEquipment"></param>
    public void CompareStat(Equipment newEquipment, Equipment equipedEquipment)
    {

        #region Armor comparing
        if (newEquipment.armor * RarityController.instance.GetRarityAmount(newEquipment)
            > equipedEquipment.armor * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            Hud_Controller.Instance.newItemArmorStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.newItemArmorStat.color = Hud_Controller.Instance.betterStatColor;
            Hud_Controller.Instance.equipedItemArmorStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.equipedItemArmorStat.color = Hud_Controller.Instance.worseStatColor;

        }
        else if (newEquipment.armor * RarityController.instance.GetRarityAmount(newEquipment)
            < equipedEquipment.armor * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            Hud_Controller.Instance.newItemArmorStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.newItemArmorStat.color = Hud_Controller.Instance.worseStatColor;
            Hud_Controller.Instance.equipedItemArmorStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.equipedItemArmorStat.color = Hud_Controller.Instance.betterStatColor;
        }
        else
        {
            Hud_Controller.Instance.newItemArmorStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.newItemArmorStat.color = Hud_Controller.Instance.equalStatColor;
            Hud_Controller.Instance.equipedItemArmorStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.equipedItemArmorStat.color = Hud_Controller.Instance.equalStatColor;
        }
        #endregion

        #region Strength comparing

        if (newEquipment.strength * RarityController.instance.GetRarityAmount(newEquipment)
            > equipedEquipment.strength * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            Hud_Controller.Instance.newItemStrengthStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.newItemStrengthStat.color = Hud_Controller.Instance.betterStatColor;
            Hud_Controller.Instance.equipedItemStrengthStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.equipedItemStrengthStat.color = Hud_Controller.Instance.worseStatColor;

        }
        else if (newEquipment.strength * RarityController.instance.GetRarityAmount(newEquipment)
            < equipedEquipment.strength * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            // El equipo equipado tiene más fuerza
            Hud_Controller.Instance.newItemStrengthStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.newItemStrengthStat.color = Hud_Controller.Instance.worseStatColor;
            Hud_Controller.Instance.equipedItemStrengthStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.equipedItemStrengthStat.color = Hud_Controller.Instance.betterStatColor;
        }
        else
        {
            // Misma fuerza
            Hud_Controller.Instance.newItemStrengthStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.newItemStrengthStat.color = Hud_Controller.Instance.equalStatColor;
            Hud_Controller.Instance.equipedItemStrengthStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.equipedItemStrengthStat.color = Hud_Controller.Instance.equalStatColor;
        }

        #endregion

        #region Speed comparing

        if (newEquipment.speed * RarityController.instance.GetRarityAmount(newEquipment)
            > equipedEquipment.speed * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            Hud_Controller.Instance.newItemSpeedStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.newItemSpeedStat.color = Hud_Controller.Instance.betterStatColor;
            Hud_Controller.Instance.equipedItemSpeedStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.equipedItemSpeedStat.color = Hud_Controller.Instance.worseStatColor;

        }
        else if (newEquipment.speed * RarityController.instance.GetRarityAmount(newEquipment)
            < equipedEquipment.speed * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            // El equipo equipado tiene más velocidad
            Hud_Controller.Instance.newItemSpeedStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.newItemSpeedStat.color = Hud_Controller.Instance.worseStatColor;
            Hud_Controller.Instance.equipedItemSpeedStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.equipedItemSpeedStat.color = Hud_Controller.Instance.betterStatColor;
        }
        else
        {
            // Misma velocidad
            Hud_Controller.Instance.newItemSpeedStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.newItemSpeedStat.color = Hud_Controller.Instance.equalStatColor;
            Hud_Controller.Instance.equipedItemSpeedStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.equipedItemSpeedStat.color = Hud_Controller.Instance.equalStatColor;
        }

        #endregion

        #region CritChance comparing
        if (newEquipment.criticChance * RarityController.instance.GetRarityAmount(newEquipment)
            > equipedEquipment.criticChance * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            // Nuevo equipo tiene mayor probabilidad de crítico
            Hud_Controller.Instance.newItemCritStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.newItemCritStat.color = Hud_Controller.Instance.betterStatColor;
            Hud_Controller.Instance.equipedItemCritChanceStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.equipedItemCritChanceStat.color = Hud_Controller.Instance.worseStatColor;
        }
        else if (newEquipment.criticChance * RarityController.instance.GetRarityAmount(newEquipment)
            < equipedEquipment.criticChance * RarityController.instance.GetRarityAmount(equipedEquipment))
        {
            // El equipo equipado tiene mayor probabilidad de crítico
            Hud_Controller.Instance.newItemCritStat.sprite = Hud_Controller.Instance.worseStatSprite;
            Hud_Controller.Instance.newItemCritStat.color = Hud_Controller.Instance.worseStatColor;
            Hud_Controller.Instance.equipedItemCritChanceStat.sprite = Hud_Controller.Instance.betterStatSprite;
            Hud_Controller.Instance.equipedItemCritChanceStat.color = Hud_Controller.Instance.betterStatColor;
        }
        else
        {
            // Igual probabilidad de crítico
            Hud_Controller.Instance.newItemCritStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.newItemCritStat.color = Hud_Controller.Instance.equalStatColor;
            Hud_Controller.Instance.equipedItemCritChanceStat.sprite = Hud_Controller.Instance.equalStatSprite;
            Hud_Controller.Instance.equipedItemCritChanceStat.color = Hud_Controller.Instance.equalStatColor;
        }
        #endregion
    }
    #endregion
}
