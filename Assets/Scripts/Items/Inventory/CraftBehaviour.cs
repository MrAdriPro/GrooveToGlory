using UnityEngine;
using NaughtyAttributes;

public class CraftBehaviour : MonoBehaviour
{
    //Variables


    //Functions

    public void CraftItem(Recipes recipe) 
    {
        // Cuando se llama a esta funcion se recorre el inventario de materiales comprobando si puedo craftear
        // Si no se puede craftear aparecera el item del inventario de crafteo como si estuviese desactivado
        // Para ello hay que crear un inventario (UI) donde se instancien todas las recipes crafteables
        // Luego de ello al seleccionar una se llama a este metodo donde pasamos la recipe que nos interesa y 
        // Comprobamos el inventario

    }
}
