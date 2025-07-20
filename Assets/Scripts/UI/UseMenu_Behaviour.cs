using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using System.Collections.Generic;

public class UseMenu_Behaviour : MonoBehaviour
{
    //Variables
    private List<Items> item;
    private Inventory inventory;
    private int slotPos;
    [SerializeField] private Button equipButton = null;
    //Functions

    private void Start()
    {
        inventory = GetComponentInParent<Inventory>();
    }


    /// <summary>
    /// Equipa el item (equipment)
    /// </summary>
    public void EquipItem() 
    {
        inventory.SetEquipment((Equipment)item[0], false, slotPos);
        
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="slot"></param>
    public void SetAttributes(List<Items> item, int slotPos) 
    {
        this.item = item;
        this.slotPos = slotPos;

        if (item[0] is Material)
            equipButton.interactable = false;
        else equipButton.interactable = true;
    }

    public List<Items> GetItems => item;

    /// <summary>
    /// Dropea el item al suelo
    /// </summary>
    public void DropItem() 
    {
        if (item[0] is Equipment)
        {
            inventory.CreateItem(item[0], null);
            inventory.RemoveEquipment((Equipment)item[0], slotPos);
        }
        else if (item[0] is Material) 
        {
            List<Material> materialList = new List<Material>();   
            foreach (var material in item)
            {
                materialList.Add((Material)material);
            }

            inventory.RemoveMaterial(materialList, slotPos, true);
        }
        this.gameObject.SetActive(false);

    }
}

