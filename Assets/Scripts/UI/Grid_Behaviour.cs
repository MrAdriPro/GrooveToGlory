using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System;

public class Grid_Behaviour : MonoBehaviour
{
    //Variables

    // Public and shown variables

    //-------------------------------------------------------------------------------------
    [Header("Inventory Slots")]
    [Space(10)]
    [SerializeField] private List<GameObject> slots; //List of slots in the inventory grid
    [SerializeField] private ItemType itemType;
    //-------------------------------------------------------------------------------------


    //-------------------------------------------------------------------------------------
    [Space(10)]
    [BoxGroup("Grid Configuration")]
    [Tooltip("Use a custom slot size for the grid. If false, the slots will be calculated based on the slot expected amount.")]
    [SerializeField] private bool useCustomSlotSize = false; //Use custom cell size for the grid

    [DisableIf("useCustomSlotSize")]
    [BoxGroup("Grid Configuration")]
    [Tooltip("Size of the grid in terms of number of slots.")]
    [SerializeField] private Vector2 gridSize = new Vector2(10, 10); //Size of the grid in terms of number of slots (columns x rows)

    [DisableIf("useCustomSlotSize")]
    [BoxGroup("Grid Configuration")]
    [Tooltip("Sets the same size for the width and height.")]
    [SerializeField] private bool sameWidtHeightSlot = false;

    [ShowIf("useCustomSlotSize")]
    [EnableIf("useCustomSlotSize")]
    [BoxGroup("Grid Configuration")]
    [Tooltip("Size of each slot in the grid. Only used if useCustomSlotSize is true.")]
    public Vector2 slotSize = new Vector2(25, 25); //Size of each slot in the grid

    [BoxGroup("Grid Configuration")]
    [Tooltip("Padding between slots in the grid.")]
    public Vector2 slotPadding = new Vector2(5, 5); //Padding between slots

    //-------------------------------------------------------------------------------------


    //-------------------------------------------------------------------------------------
    [Space(10)]
    [Header("Prefabs")]
    [BoxGroup("Prefabs")]
    [Tooltip("Prefab for the slot in the inventory grid.")]
    public GameObject slotPrefab;

    [BoxGroup("Prefabs")]
    [Tooltip("Prefab for the inventory UI.")]
    public GameObject inventoryUI; //Inventory UI prefab


    [BoxGroup("Prefabs")]
    [Tooltip("Container for the inventory UI. If not assigned, it will use the inventory UI RectTransform.")]
    [SerializeField] private RectTransform container;

    [BoxGroup("Prefabs")]
    [Tooltip("Container for the items to create.")]
    [SerializeField] private RectTransform itemContainer;

    //------------------------------------------------------------------------------------- 


    //Functions
    void Start()
    {
        InstantiateSlots();

        if(itemType == ItemType.Material)
            GetComponentInParent<Inventory>().SetMaterialSlots(GetSlots());

        if (itemType == ItemType.Equipment)
            GetComponentInParent<Inventory>().SetEquipmentSlots(GetSlots());
    }


    /// <summary>
    /// Devuelve los slots del grid
    /// </summary>
    /// <returns></returns>
    public List<Slot_Behaviour> GetSlots() 
    {
        List<Slot_Behaviour> slotsBehaviours = new List<Slot_Behaviour>();
        foreach (var slot in slots)
        {
            slotsBehaviours.Add(slot.GetComponent<Slot_Behaviour>());
        }

        return slotsBehaviours;
    }

    /// <summary>
    /// Set all array to 0s
    /// </summary>
    /// <param name="array"></param>
    void ClearArray(ref Vector2[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = Vector2.zero;
        }
    }


    /// <summary>
    /// Instantiates the slots in the inventory grid.
    /// </summary>
    private void InstantiateSlots()
    {
        //Checks the errors for the inventory UI, container and slot prefab
        if (inventoryUI == null)
        {
            Debug.LogError("Inventory UI prefab is not assigned.");
            return;
        }
        else if (container == null)
        {
            Debug.LogError("Container is not assigned.");
            return;
        }
        else if (slotPrefab == null)
        {
            Debug.LogError("Slot prefab is not assigned.");
            return;
        }
        else container = container == null ? inventoryUI.GetComponent<RectTransform>() : container; //If the container is not assigned, use the inventory UI RectTransform


        //Checks 
        if (useCustomSlotSize)gridSize = SetGrid();
        else SetSlotSize(); //Set the slot size based on the grid size
        

        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject slot = Instantiate(slotPrefab, container);
                slot.name = "Slot_" + x + "_" + y; //Name the slot according to its position in the grid
                slot.transform.SetParent(container, false);
                slots.Add(slot); //Add the slot to the list of slots
            }

        }
    }

    /// <summary>
    /// Sets the grid size and padding based on the slots size.
    /// </summary>
    /// <returns></returns>
    private Vector2 SetGrid()
    {
        //Get the size of the grid
        RectTransform rt = inventoryUI.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        int columns = (int)gridSize.x;
        int rows = (int)gridSize.y;

        //Calculates the columns and rows of the grid based on the size of the slots and the padding
        if (useCustomSlotSize)
        {
            columns = Mathf.FloorToInt(width / (slotSize.x + slotPadding.x));
            rows = Mathf.FloorToInt(height / (slotSize.y + slotPadding.y));
        }

        // Gets the total width and height of the grid based on the number of columns and rows
        float totalWidth = columns * slotSize.x + (columns - 1) * slotPadding.x;
        float totalHeight = rows * slotSize.y + (rows - 1) * slotPadding.y;

        // Checks the padding based of the size - totalSize / 2 (to center the content)
        int horizontalPadding = Mathf.FloorToInt((width - totalWidth) / 4f);
        int verticalPadding = Mathf.FloorToInt((height - totalHeight) / 4f);

        if(!sameWidtHeightSlot)
        slotSize = new Vector2(slotSize.x + (horizontalPadding / 2), slotSize.y + (verticalPadding / 2));
        else slotSize = new Vector2(slotSize.x + (horizontalPadding / 2), slotSize.y + (horizontalPadding / 2));

        // Sets the grid parameters
        var layout = container.GetComponent<GridLayoutGroup>();
        layout.cellSize = slotSize;
        layout.spacing = slotPadding;
        layout.padding = new RectOffset(horizontalPadding, horizontalPadding, verticalPadding, verticalPadding);

        return new Vector2(columns, rows);
    }




    /// <summary>
    /// Sets the slot size and padding based on the desired number of columns and rows.
    /// </summary>
    private void SetSlotSize()
    {
        // Get the size of the grid (container)
        RectTransform rt = inventoryUI.GetComponent<RectTransform>();
        float width = rt.rect.width;
        float height = rt.rect.height;

        // Calculate the total horizontal and vertical padding space (spacing between slots)
        float totalHorizontalPadding = (gridSize.x - 1) * (slotPadding.x);
        float totalVerticalPadding = (gridSize.y - 1) * (slotPadding.y);

        // Calculate the available width and height for slots (after padding is subtracted)
        float availableWidth = (width - totalHorizontalPadding);
        float availableHeight = (height - totalVerticalPadding);

        // Calculate slot size based on available space
        float slotWidth = availableWidth / (gridSize.x + 1);
        float slotHeight = availableHeight / (gridSize.y + 1);

        // Save the slot size
        slotSize = new Vector2(slotWidth, slotHeight = sameWidtHeightSlot ? slotWidth : slotHeight);


        gridSize = SetGrid();
    }


}



