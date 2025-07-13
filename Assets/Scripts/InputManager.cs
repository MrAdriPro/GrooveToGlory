using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode leftKey = KeyCode.LeftArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode downKey = KeyCode.DownArrow;


    void Update()
    {
        if (Input.GetKeyDown(leftKey)) CheckNote("left");
        if (Input.GetKeyDown(downKey)) CheckNote("down");
        if (Input.GetKeyDown(upKey)) CheckNote("up");
        if (Input.GetKeyDown(rightKey)) CheckNote("right");
    }

    void CheckNote(string dir)
    {
        foreach (Note note  in new List<Note>(GameManager.instance.activeNotes))
        {
            if (note != null && note.direction == dir && note.canBePressed)
            {
                GameManager.instance.HitNote(note);
                return;
            }
        }

    }
}
