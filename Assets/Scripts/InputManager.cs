using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public KeyCode leftKey = KeyCode.D;
    public KeyCode rightKey = KeyCode.K;
    public KeyCode upKey = KeyCode.J;
    public KeyCode downKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(leftKey)) CheckNote("left");
        if (Input.GetKeyDown(downKey)) CheckNote("down");
        if (Input.GetKeyDown(upKey)) CheckNote("up");
        if (Input.GetKeyDown(rightKey)) CheckNote("right");
    }

    void CheckNote(string dir)
    {
        bool hit = false;
        foreach (Note note in new List<Note>(GameManager.instance.activeNotes))
        {
            if (note.direction == dir && note.canBePressed && !note.resolved)
            {
                GameManager.instance.HitNote(note);
                hit = true;
                break;
            }
        }

        if (!hit)
        {
            Debug.Log("Fallaste la nota");
            GameManager.instance.MissNote();
        }
    }
}
