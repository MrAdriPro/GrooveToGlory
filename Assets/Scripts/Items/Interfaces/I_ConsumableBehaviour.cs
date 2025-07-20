using UnityEngine;
using NaughtyAttributes;

public interface I_ConsumableBehaviour: I_GenericMethods
{
    /// <summary>
    /// Usa el consumible, y añade la cantidad dependiendo si es temporal o no
    /// </summary>
    /// <param name="consumable"></param>
    public void UseConsumable(Consumable consumable);

    /// <summary>
    /// Introduce los stats a cambiar
    /// </summary>
    /// <param name="consumable"></param>
    /// <param name="consumableStatsToRestore"></param>
    public void ConsumableStatsController(Consumable consumable, ref int[] consumableStatsToRestore);

    /// <summary>
    /// Metodo que inicia el contador para cada efecto
    /// </summary>
    /// <param name="effectTime"></param>
    /// <param name="amounts"></param>
    public void TemporalConsumableBehaviour(float effectTime, int[] amounts);
}
