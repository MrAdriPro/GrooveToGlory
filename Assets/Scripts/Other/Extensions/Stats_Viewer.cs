using UnityEngine;
using NaughtyAttributes;

public class Stats_Viewer : MonoBehaviour
{
    //Variables
    [SerializeField] private bool E_button_chest = Extensions.Chest_Indicator;
    [SerializeField] private bool E_button_item = Extensions.Item_Indicator;


    //Functions

    private void Update()
    {
        E_button_chest = Extensions.Chest_Indicator;
        E_button_item = Extensions.Item_Indicator;
    }
}
