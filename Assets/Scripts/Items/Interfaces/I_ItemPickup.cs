using UnityEngine;
using NaughtyAttributes;

public interface I_ItemPickup : I_GenericMethods
{
    /// <summary>
    /// Controlador de los inputs
    /// </summary>
    public void InputsController();
    /// <summary>
    /// Un metodo demasiado largo con el raycast para comprobar la posicion del raton y permitir interactuar con los objetos
    /// </summary>
    void OnItemHover();
    /// <summary>
    /// Metodo que pone los estados necesarios a falso
    /// </summary>
    void SetObjectStatusToFalse();
    /// <summary>
    /// Obtiene el objeto
    /// </summary>
    void PickupItem();
    /// <summary>
    /// Establece el status del item
    /// </summary>
    /// <param name="item"></param>
    void SetItemStatus(GenericItemHolder item);
    /// <summary>
    /// Establece el status del item
    /// </summary>
    /// <param name="status"></param>
    void SetItemStatus(bool status);
    /// <summary>
    /// Obtiene la informacion del objeto
    /// </summary>
    void GetItem();
    /// <summary>
    /// Destruye el item del mundo
    /// </summary>
    void DeleteItemFromWorld();
    /// <summary>
    /// Metodo que establece que el objeto se ha comprado
    /// </summary>
    /// <param name="objectPicked"></param>
    void ObjectBuyed(bool objectPicked);
    /// <summary>
    /// Metodo que establece que el objeto se ha recogido
    /// </summary>
    /// <param name="objectPicked"></param>
    void ObjectPicked(bool objectPicked);
    /// <summary>
    /// Spawnea el texto de info
    /// </summary>
    /// <param name="message"></param>
    public void InfoText(string message, Color color, float time);
    /// <summary>
    /// Spawnea el texto de info
    /// </summary>
    /// <param name="message"></param>
    public void InfoText(string message, Color color);
}
