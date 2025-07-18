using UnityEngine;

[CreateAssetMenu(fileName = "Note", menuName = "Notes/New Note")]
public class Note_SO : ScriptableObject
{
    public NoteDirection direction;           
    public float speed = 5f;
    public float lifeTime = 5f;
    public bool isDangerous = false;
}
public enum NoteDirection : byte
{
    Left,
    Right,
    Up,
    Down,
    None
}