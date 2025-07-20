using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableBehaviour : MonoBehaviour, I_ConsumableBehaviour
{
    #region Variables

    [Header("Private variables")]
    [SerializeField] private List<GameObject> consumableTimes = new List<GameObject>();
    private Inventory inventory;
    private Transform _temporalConsumibleParent = null;

    #endregion

    #region Functions

    private void Start() => SetStartingAttributes();
    

    /// <summary>
    /// Establece las variables de inicio
    /// </summary>
    public void SetStartingAttributes() 
    {
        _temporalConsumibleParent = GameObject.FindGameObjectWithTag(Tags.TEMPORAL_CONS_PARENT_TXT_TAG).GetComponent<Transform>();
        inventory = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Inventory>();
    }

    /// <summary>
    /// Usa el consumible, y añade la cantidad dependiendo si es temporal o no
    /// </summary>
    /// <param name="consumable"></param>
    public void UseConsumable(Consumable consumable) 
    {
        int[] consumableStatsToRestore = new int[Enum.GetValues(typeof(ConsumableUses)).Length];

        if (consumable.consumableType.Equals(ConsumableType.Temporal))
        {
            if (!_temporalConsumibleParent) return;

            ConsumableStatsController(consumable, ref consumableStatsToRestore);
            TemporalConsumableBehaviour(consumable.consumableTimeEffect, consumableStatsToRestore);
        }
        else ConsumableStatsController(consumable, ref consumableStatsToRestore);

    }

    /// <summary>
    /// Introduce los stats a cambiar
    /// </summary>
    /// <param name="consumable"></param>
    /// <param name="consumableStatsToRestore"></param>
    public void ConsumableStatsController(Consumable consumable, ref int[] consumableStatsToRestore) 
    {
        switch (consumable.consumableUses)
        {
            case ConsumableUses.RestoresHealth:
                consumableStatsToRestore[(int)ConsumableUses.RestoresHealth] += consumable.healthToRestore;
                break;
            case ConsumableUses.AddsArmor:
                consumableStatsToRestore[(int)ConsumableUses.AddsArmor] += consumable.armorToAdd;
                break;
            case ConsumableUses.AddStrenth:
                consumableStatsToRestore[(int)ConsumableUses.AddStrenth] += consumable.strengthToAdd;
                break;
            case ConsumableUses.AddSpeed:
                consumableStatsToRestore[(int)ConsumableUses.AddSpeed] += consumable.speedToAdd;
                break;
            case ConsumableUses.AddCritChance:
                consumableStatsToRestore[(int)ConsumableUses.AddCritChance] += consumable.critChanceToAdd;
                break;
            case ConsumableUses.AddMultiple:
                consumableStatsToRestore[(int)ConsumableUses.RestoresHealth] += consumable.healthToRestore;
                consumableStatsToRestore[(int)ConsumableUses.AddsArmor] += consumable.armorToAdd;
                consumableStatsToRestore[(int)ConsumableUses.AddStrenth] += consumable.strengthToAdd;
                consumableStatsToRestore[(int)ConsumableUses.AddSpeed] += consumable.speedToAdd;
                consumableStatsToRestore[(int)ConsumableUses.AddCritChance] += consumable.critChanceToAdd;
                break;
            default:
                break;
        }

        inventory.UpdateStats(consumableStatsToRestore);

    }

    /// <summary>
    /// Metodo que inicia el contador para cada efecto
    /// </summary>
    /// <param name="effectTime"></param>
    /// <param name="amounts"></param>
    public void TemporalConsumableBehaviour(float effectTime, int[] amounts) 
    {
        GameObject newEffect = new GameObject("Consumable_Effect_" + consumableTimes.Count.ToString()); // CAMBIAR POR PREFAB
        newEffect.transform.SetParent(_temporalConsumibleParent);
        TemporalConsumable_Behaviour component = newEffect.AddComponent<TemporalConsumable_Behaviour>();
        component.Init(effectTime, amounts);
    }
    #endregion
}

public class TemporalConsumable_Behaviour : MonoBehaviour
{

    private int[] amounts;
    private float consumableTime;
    public float currentTime = 0;

    /// <summary>
    /// Inicia el contador del consumible temporal
    /// </summary>
    /// <param name="consumableTime"></param>
    /// <param name="amounts"></param>
    public void Init(float consumableTime, int[] amounts) 
    {
        this.consumableTime = consumableTime;
        this.amounts = amounts;
        StartCoroutine(Countdown());
    }

    /// <summary>
    /// Corrutina del contador
    /// </summary>
    /// <returns></returns>
    public IEnumerator Countdown() 
    {
        currentTime = consumableTime;

        while (currentTime > 0) 
        {
            currentTime -= Time.deltaTime;
            yield return null;
        }

        ResetStatsAdded(amounts);
    }

    /// <summary>
    /// Resetea los stats
    /// </summary>
    /// <param name="amount"></param>
    public void ResetStatsAdded(int[] amount) 
    {
        Player_Stats.armor -= amount[(int)ConsumableUses.AddsArmor];
        Player_Stats.strength -= amount[(int)ConsumableUses.AddStrenth];
        Player_Stats.speed -= amount[(int)ConsumableUses.AddSpeed];
        Player_Stats.criticChance -= amount[(int)ConsumableUses.AddCritChance];

        Hud_Controller.Instance.SetText(Player_Stats.armor, Player_Stats.strength, Player_Stats.speed, Player_Stats.criticChance);

        Destroy(this.gameObject);
    }
}
