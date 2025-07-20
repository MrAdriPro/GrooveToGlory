using UnityEngine;
using NaughtyAttributes;

public interface I_CursorController : I_GenericMethods
{
    /// <summary>
    /// Mueve el cursor donde estaria el nuestro
    /// </summary>
    public void MouseCursor();

    /// <summary>
    /// Pone el boton de interaccion a activo
    /// </summary>
    /// <param name="active"></param>
    public void SetCursor(CursorType cursor);
}
