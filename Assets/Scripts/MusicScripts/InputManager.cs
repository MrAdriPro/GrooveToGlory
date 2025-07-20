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
        //lo que pasa que la del esqueleto solo funciona con la direccion que le pongas 
        //TODO: Flecha esqueleto que cambie en la dirección en la que spawnee
        if (Input.GetKeyDown(leftKey)) CheckNote(NoteDirection.Left);
        if (Input.GetKeyDown(downKey)) CheckNote(NoteDirection.Down);
        if (Input.GetKeyDown(upKey)) CheckNote(NoteDirection.Up);
        if (Input.GetKeyDown(rightKey)) CheckNote(NoteDirection.Right);
    }

    void CheckNote(NoteDirection dir)
    {
        foreach (Note note in FightManager.instance.activeNotes)
        {
            if (note.note.direction == dir && note.canBePressed && !note.resolved)
            {
                FightManager.instance.HitNote(note);
                return;
            }
        }

        FightManager.instance.MissNote(null);
    }

}
