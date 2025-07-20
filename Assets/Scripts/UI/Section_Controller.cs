using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class Section_Controller : MonoBehaviour
{
    //Variables
    [BoxGroup("Sections Settings")]
    public GameObject sectionMenu;
    private Image sectionImage;
    private TextMeshProUGUI sectionTitle;

    public List<Section> sections = new List<Section>();

    public List<GameObject> menus = new List<GameObject>();
    private int index = 0;
    //Functions

    private void Start()
    {
        if (sectionMenu)
        {
            sectionImage = sectionMenu.transform.GetChild(0).GetComponent<Image>();
            sectionTitle = sectionMenu.GetComponentInChildren<TextMeshProUGUI>();
            SetSectionActive();
        }
        
    }

    public void SetSection() 
    {

    }

    public void NextSectionLeft() 
    {
        index--;
        if (index < 0) index = sections.Count - 1;
        SetSectionActive();
    }

    public void NextSectionRight()
    {
        index++;
        if(index > sections.Count - 1) index = 0;
        SetSectionActive();
    }

    private void SetSectionActive() 
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (i == index)
            {
                menus[i].SetActive(true);
                if (sectionMenu)
                {
                    sectionImage.sprite = sections[i].sectionIcon;
                    sectionTitle.text = sections[i].sectionTitle;
                }
            }
            else 
            {
                menus[i].SetActive(false);
            }
        }
    }
}
