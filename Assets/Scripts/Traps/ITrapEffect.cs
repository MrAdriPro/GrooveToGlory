using UnityEngine;

/// <summary>
/// Interface for trap effects.
/// Implement this to define custom behaviourr when a trap is activated.
/// </summary>
public interface ITrapEffect
{
    /// <summary>
    /// Called when the trap is triggered.
    /// Use playerHealth to apply effects to the player.
    /// Use data for trap-specific parameters.
    /// </summary>
    void Activate(PlayerHealth playerHealth, TrapData data);
}
