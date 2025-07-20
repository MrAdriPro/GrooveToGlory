using UnityEngine;
using NaughtyAttributes;
using System;

public class Room_cell : MonoBehaviour
{
    public int id; //ID of the item
    public string roomName; //Name of the item
    public string description; //Description of the item

    public Vector2 roomcellPosition;

    //Constructor
    public Room_cell(int id, string itemName, string description, Vector2 roomcellPosition)
    {
        this.id = id; //Set the ID of the item
        this.roomName = itemName; //Set the name of the item
        this.description = description; //Set the description of the item
        this.roomcellPosition = roomcellPosition;
    }

    private void Start()
    {
        roomcellPosition = new Vector2(Int32.Parse(gameObject.name.Split("_")[1]), Int32.Parse(gameObject.name.Split("_")[2]));
    }


    public bool IsEmpty() => id != 0 ? false : true;
}
