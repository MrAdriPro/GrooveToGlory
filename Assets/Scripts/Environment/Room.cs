using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Room", menuName = "Room/New Room")]
public class Room : ScriptableObject
{
    //Variables

    public string roomName = string.Empty;
    public int roomId = 1;
    public RoomType roomType;
    public GameObject roomObject;
    public Vector2 roomSize;
    public float roomWidth;

    //Functions
}

public enum RoomType 
{
    Normal,
    Store,
    Boss,
    NoEnemies
}
