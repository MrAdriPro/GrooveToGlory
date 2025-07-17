using System;
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
        if (Input.GetKeyDown(leftKey)) CheckNote(NoteDirection.Left);
        if (Input.GetKeyDown(downKey)) CheckNote(NoteDirection.Down);
        if (Input.GetKeyDown(upKey)) CheckNote(NoteDirection.Up);
        if (Input.GetKeyDown(rightKey)) CheckNote(NoteDirection.Right);
    }

    void CheckNote(NoteDirection dir)
    {
        bool hit = false;
        foreach (Note note in FightManager.instance.activeNotes)
        {
            if (note.note.direction == dir)
            {
                if (note.canBePressed && !note.resolved)
                {
                    FightManager.instance.HitNote(note);
                    hit = true;
                    break;
                }
            }
        }

        if (!hit)
        {
            Debug.Log("Fallaste la nota");
            FightManager.instance.MissNote(null);
        }
    }
}
