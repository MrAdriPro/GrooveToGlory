using UnityEngine;
using NaughtyAttributes;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;
using UnityEditor;
using System.Collections;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI;
using System.Collections.Generic;

public class Hud_Controller : MonoBehaviour
{
    #region Variables
    [Header("Instance")]
    public static Hud_Controller Instance;

    //Variables
    [Header("HUD")]
    [BoxGroup("Menus")]
    public GameObject hud = null;
    [BoxGroup("Menus")]
    public GameObject comparePanel = null;

    [BoxGroup("Menus")]
    [SerializeField] GameObject pauseMenu;
    [BoxGroup("Menus")]
    [SerializeField] M_MainMenu mainMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject matMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject equMenu;
    [BoxGroup("Menus")]
    public GameObject statsMenu;
    [BoxGroup("Menus")]
    [SerializeField] private GameObject itemUseMenu;
    [BoxGroup("Menus")]
    public Vector2 comparePanelOffset = Vector2.zero;

    [BoxGroup("Others")]
    public Image mouseCursor = null;
    [BoxGroup("Others")]
    public TextMeshProUGUI goldCoinText = null;
    [HideInInspector] public StatsMenuController statsMenuController = null;

    [BoxGroup("New Item Info")]
    public GameObject newItemComparePanel = null;
    [BoxGroup("New Item Info")]
    public Image newItemIcon = null;
    [BoxGroup("New Item Info")]
    public TextMeshProUGUI newItemName = null;
    [BoxGroup("New Item Info")]
    public TextMeshProUGUI newItemArmor = null;
    [BoxGroup("New Item Info")]
    public Image newItemArmorStat = null;
    [BoxGroup("New Item Info")]
    public TextMeshProUGUI newItemStrength = null;
    [BoxGroup("New Item Info")]
    public Image newItemStrengthStat = null;
    [BoxGroup("New Item Info")]
    public TextMeshProUGUI newItemSpeed = null;
    [BoxGroup("New Item Info")]
    public Image newItemSpeedStat = null;
    [BoxGroup("New Item Info")]
    public TextMeshProUGUI newItemCritChance = null;
    [BoxGroup("New Item Info")]
    public Image newItemCritStat = null;

    [BoxGroup("Equiped Item Info")]
    public GameObject equipedItemComparePanel = null;
    [BoxGroup("Equiped Item Info")]
    public Image equipedItemIcon = null;
    [BoxGroup("Equiped Item Info")]
    public TextMeshProUGUI equipedItemName = null;
    [BoxGroup("Equiped Item Info")]
    public TextMeshProUGUI equipedItemArmor = null;
    [BoxGroup("Equiped Item Info")]
    public Image equipedItemArmorStat = null;
    [BoxGroup("Equiped Item Info")]
    public TextMeshProUGUI equipedItemStrength = null;
    [BoxGroup("Equiped Item Info")]
    public Image equipedItemStrengthStat = null;
    [BoxGroup("Equiped Item Info")]
    public TextMeshProUGUI equipedItemSpeed = null;
    [BoxGroup("Equiped Item Info")]
    public Image equipedItemSpeedStat = null;
    [BoxGroup("Equiped Item Info")]
    public TextMeshProUGUI equipedItemCritChance = null;
    [BoxGroup("Equiped Item Info")]
    public Image equipedItemCritChanceStat = null;

    [BoxGroup("Stats Icon Customizer")]
    public Color betterStatColor = Color.green;
    [BoxGroup("Stats Icon Customizer")]
    public Sprite betterStatSprite = null;
    [BoxGroup("Stats Icon Customizer")]
    public Color worseStatColor = Color.red;
    [BoxGroup("Stats Icon Customizer")]
    public Sprite worseStatSprite = null;
    [BoxGroup("Stats Icon Customizer")]
    public Color equalStatColor = Color.gray;
    [BoxGroup("Stats Icon Customizer")]
    public Sprite equalStatSprite = null;


    [Header("Stats")]
    private TextMeshProUGUI armor = null;
    private TextMeshProUGUI strength = null;
    private TextMeshProUGUI speed = null;
    private TextMeshProUGUI critChance = null;


    [Header("Armor cells")]
    public Image[] itemIconcells = null;

    [Header("Consumables")]
    public Image[] consumableIcons = null;

    [Header("Private variables")]
    [SerializeField] private bool isInMenu = false;
    private Transform playerBody = null;
    private M_PauseMenu m_PauseMenu;

    #endregion

    private void Awake()
    {
        if (Instance) Destroy(this);
        else Instance = this;
    }

    //Variables
    private void Start() => SetStartingAttributes();


    private void Update() => InputsController();


    /// <summary>
    /// Controlador de los inputs
    /// </summary>
    public void InputsController() 
    {
        if (isInMenu) return;

        if (Input.GetKeyDown(Player_Inputs.OpenInventory)) InventoryActive();
        

        if (Input.GetKeyDown(Player_Inputs.PauseMenu))
        {
            if (!Inventory_Stats.inventoryOpened)
            {

                if (pauseMenu.activeSelf) mainMenu.OnMouseButton_ContinuePauseMenu();
                else
                {
                    try
                    {
                        pauseMenu.SetActive(!pauseMenu.activeSelf);
                        mainMenu.MoveSelectorToButton(mainMenu.GetFirstMenuButton(M_MainMenu.Menus.Main));
                    }
                    catch (Exception) { }
                }
            }
            else InventoryActive();

        }
    }


    public void UpdateGoldCoinsText(int amount) => goldCoinText.text = amount.ToString();


    /// <summary>
    /// Abre o cierra el inventario
    /// </summary>
    public void InventoryActive()
    {
        if (Extensions.isPauseMenuOpened) return;

        Inventory_Stats.inventoryOpened = !Inventory_Stats.inventoryOpened;
        hud.SetActive(Inventory_Stats.inventoryOpened);

        if (Inventory_Stats.inventoryOpened)
        {
            if (statsMenu.activeSelf)
            {
                armor = GameObject.FindGameObjectWithTag(Tags.ARMOR_TXT_TAG).GetComponent<TextMeshProUGUI>();
                strength = GameObject.FindGameObjectWithTag(Tags.STRENGTH_TXT_TAG).GetComponent<TextMeshProUGUI>();
                speed = GameObject.FindGameObjectWithTag(Tags.SPEED_TXT_TAG).GetComponent<TextMeshProUGUI>();
                critChance = GameObject.FindGameObjectWithTag(Tags.CRITCHANCE_TXT_TAG).GetComponent<TextMeshProUGUI>();

                SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="active"></param>
    public void SetUseItemMenu(bool active, List<Items> item, int slotPos)
    {
        if (item != null)
        {
            itemUseMenu.GetComponent<UseMenu_Behaviour>().SetAttributes(item, slotPos);
        }
        itemUseMenu.SetActive(active);
    }

    /// <summary>
    /// Establece los atributos necesarios al iniciar
    /// </summary>
    public void SetStartingAttributes()
    {
        statsMenuController = GetComponent<StatsMenuController>();
        playerBody = GameObject.FindGameObjectWithTag(Tags.PLAYER_BODY_TAG).GetComponent<Transform>();
        m_PauseMenu = FindAnyObjectByType<M_PauseMenu>();

        SetMenusStart();
    }

    public void SetMenusStart() 
    {
        if (isInMenu) return;

        matMenu.SetActive(false);
        statsMenu.SetActive(false);
        equMenu.SetActive(false);
        itemUseMenu.SetActive(false);
        hud.SetActive(false);
        pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Activa o desactiva el inventario de materiales
    /// </summary>
    public void SetMenu(int index)
    {
        switch (index)
        {
            case (int)InventoryMenus.Materials:
                matMenu.SetActive(!matMenu.activeSelf);
                break;
            case (int)InventoryMenus.Equipment:
                equMenu.SetActive(!equMenu.activeSelf);
                break;
            default:
                break;
        }

        
    }

    /// <summary>
    /// Activa o desactiva las stats
    /// </summary>
    public void SetStatsMenu()
    {
        statsMenu.SetActive(!statsMenu.activeSelf);

        if (!statsMenu.activeSelf) return;


        armor = GameObject.FindGameObjectWithTag(Tags.ARMOR_TXT_TAG).GetComponent<TextMeshProUGUI>();
        strength = GameObject.FindGameObjectWithTag(Tags.STRENGTH_TXT_TAG).GetComponent<TextMeshProUGUI>();
        speed = GameObject.FindGameObjectWithTag(Tags.SPEED_TXT_TAG).GetComponent<TextMeshProUGUI>();
        critChance = GameObject.FindGameObjectWithTag(Tags.CRITCHANCE_TXT_TAG).GetComponent<TextMeshProUGUI>();

        SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);
    }

    public void SetText(int _armor, int _strength, int _speed, int _critChance) 
    {
        if (!armor || !strength || !speed || !critChance) return;

        armor.text = Extensions.ARMOR_TXT + _armor;
        strength.text = Extensions.STRENGTH_TXT + _strength;
        speed.text = Extensions.SPEED_TXT + _speed;
        critChance.text = Extensions.CRITCHANGE_TXT + _critChance;

    }

    /// <summary>
    /// Actualiza los iconos del inventario
    /// </summary>
    /// <param name="equipment"></param>
    public void UpdateItemIcons(Equipment[] equipment) 
    {
        for (int i = 0; i < itemIconcells.Length; i++)
        {
            if (equipment[i] != null) SetItemIcon(equipment[i], i, true);
            else SetItemIcon(equipment[i], i, false);
        }
    }

    /// <summary>
    /// Actualiza los iconos del inventario
    /// </summary>
    /// <param name="equipment"></param>
    public void UpdateItemIcons(Consumable[] consumables)
    {
        for (int i = 0; i < consumableIcons.Length; i++)
        {
            if (consumables[i] != null) SetItemIcon(consumables[i], i, true);
            else SetItemIcon(consumables[i], i, false);
        }
    }


    /// <summary>
    /// Pone el icono del cell
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="position"></param>
    /// <param name="active"></param>
    private void SetItemIcon(Equipment equipment,int position, bool active) 
    {
        itemIconcells[position].gameObject.SetActive(active);
        itemIconcells[position].sprite = active ? equipment.itemIcon : null;
    }

    /// <summary>
    /// Pone el icono del cell
    /// </summary>
    /// <param name="equipment"></param>
    /// <param name="position"></param>
    /// <param name="active"></param>
    private void SetItemIcon(Consumable consumable, int position, bool active)
    {
        consumableIcons[position].gameObject.SetActive(active);
        consumableIcons[position].sprite = active ? consumable.itemIcon : null;
    }



    /// <summary>
    /// Obtener el menu de pausa
    /// </summary>
    public GameObject GetPauseMenu => pauseMenu;

}

public enum CursorType 
{
    Normal,
    Use,
    Hover
}

public enum InventoryMenus 
{
    Materials,
    Equipment
}