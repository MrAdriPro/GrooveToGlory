using UnityEngine;
using NaughtyAttributes;

public interface I_ItemDrop : I_GenericMethods
{
    //Variables
    /// <summary>
    /// Metodo que inicia todo los datos necesarios para la animacion
    /// </summary>
    public void SetStartingDropSettings(Items item);
    /// <summary>
    /// Establece una posicion aleatoria al dropear un item
    /// </summary>
    /// <param name="item"></param>
    public void SetRandomPosition(Items item);
    /// <summary>
    /// Metodo que controla la animacion de la caida del objeto
    /// </summary>
    public void AnimationController();
}
