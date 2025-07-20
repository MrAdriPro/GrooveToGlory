using UnityEngine;

[CreateAssetMenu(menuName = "Traps/Trap Effect Handler")]
public class TrapEffectHandler : ScriptableObject, ITrapEffect
{
    /// <summary>
    /// This is only the method of the interface that will be called when the trap is activated.
    /// </summary>
    /// <param name="playerHealth"></param>
    /// <param name="data"></param>
    public void Activate(PlayerHealth playerHealth, TrapData data)
    {
        switch (data.trapType)
        {
            case TrapType.Spikes:
                HandleSpikes(playerHealth, data);
                break;

            case TrapType.Fire:
                HandleFire(playerHealth, data);
                break;

            case TrapType.Poison:
                HandlePoison(playerHealth, data);
                break;

            default:
                Debug.LogWarning("No effect defined for trap type: " + data.trapType);
                break;
        }
    }

    /// <summary>
    /// The methonds below are the specific effects for each trap type.
    /// They can be modified to add more complex behaviors or effects.
    /// </summary>
    /// <param name="playerHealth"></param>
    /// <param name="data"></param>
    private void HandleSpikes(PlayerHealth playerHealth, TrapData data)
    {
        playerHealth.TakeDamage(data.damage);
        Debug.Log($" Daño al jugador: {data.damage}");
    }

    private void HandleFire(PlayerHealth playerHealth, TrapData data)
    {
        // Remember to change here de damage value if we put the burnDamage in the TrapData
        float burnDamage = data.damage * 2f;
        playerHealth.TakeDamage(burnDamage);
        Debug.Log($" Quemado al jugador: {burnDamage}");
    }

    private void HandlePoison(PlayerHealth playerHealth, TrapData data)
    {
        float poisonDamage = data.damage / 2f;
        playerHealth.TakeDamage(poisonDamage);
        Debug.Log($" Envenenado al jugador: {poisonDamage}");
    }
}
