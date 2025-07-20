using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using System.Collections;

public class Slot_Behaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //Variables
    public Image itemImage;
    public TextMeshProUGUI itemAmountText;

    [BoxGroup("Slot info")]
    public List<Items> itemsInSlot;
    [BoxGroup("Slot info")]
    public Sprite slotBackground;

    [HideInInspector] public int slotPos = 0;

    /// <summary>
    /// Devuelve si el slot esta vacio
    /// </summary>
    public bool IsEmpty => itemsInSlot.Count == 0;

    /// <summary>
    /// Devuelve el primer material
    /// </summary>
    public Items GetItemInSlot => itemsInSlot[0] != null ? itemsInSlot[0] : null;

    //Functions

    private void Update()
    {

        itemAmountText.text = itemAmountText.text.Equals("0") || itemAmountText.text.Equals("1") ? string.Empty : itemAmountText.text;

        
       if(!Extensions.useMenuActive && Input.GetKeyDown(Player_Inputs.RightClick) && Extensions.slotChecking != gameObject)
       Hud_Controller.Instance.SetUseItemMenu(Extensions.useMenuActive, null, 0);

        if (Input.GetKeyDown(Player_Inputs.LeftClick)) StartCoroutine(CloseUseItemMenu());
    }

    /// <summary>
    /// Sets the item icon
    /// </summary>
    /// <param name="status"></param>
    /// <param name="itemIcon"></param>
    public void SetItemIcon(bool status, Sprite itemIcon) 
    {
        if (!status) itemImage.color = new Color(0,0,0,0);
        else itemImage.color = Color.white;

        itemImage.sprite = itemIcon;
    }

    /// <summary>
    /// Sets the item amount text
    /// </summary>
    /// <param name="amount"></param>
    public void SetItemAmountText(int amount) => itemAmountText.text = amount.ToString();

    public void OnPointerEnter(PointerEventData eventData)
    {
        Extensions.slotChecking = this.gameObject;

        if (itemsInSlot.Count != 0)
        {

            if (itemsInSlot[0] is Equipment)
            {
                Equipment equipment = itemsInSlot[0] as Equipment;
                Hud_Controller.Instance.statsMenuController.ActivateInfoCompareHud(
                    equipment,
                    GetComponentInParent<Inventory>().GetEquipmentByPosition((int)equipment.placeholder), GetComponent<RectTransform>());
            }
        }
    }



    public void OnPointerExit(PointerEventData eventData)
    {
        if (Extensions.slotChecking == gameObject) 
        {
            Hud_Controller.Instance.statsMenuController.ActivateInfoCompareHud(null, null);
            Extensions.slotChecking = null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Extensions.slotChecking == gameObject)
            {
                if (itemsInSlot.Count > 0)
                {
                    Extensions.slotChecking = null;
                    Extensions.useMenuActive = false;
                    Hud_Controller.Instance.SetUseItemMenu(true, itemsInSlot, slotPos);
                    Hud_Controller.Instance.statsMenuController.ActivateInfoCompareHud(null, null);
                }
            }
                

        }

    }

    /// <summary>
    /// Espera para cerrar el menu de uso del objeto
    /// </summary>
    /// <returns></returns>
    private IEnumerator CloseUseItemMenu() 
    {
        yield return new WaitForSeconds(0.1f);
        Hud_Controller.Instance.SetUseItemMenu(Extensions.useMenuActive, null, 0);
    }

}
