using UnityEngine;

/// <summary>
/// Data class for traps.
/// Contains properties like damage and type of trap.
/// If we want expecific datas for a trap remember to use SHOW IF and 
/// #pragma warning disable
/// private bool IsZombie => enemyType.Equals(EnemyType.Zombie); this is an example of how to use it.
//////#pragma warning restore
/// </summary>
[CreateAssetMenu(menuName = "Traps/New Trap")]
public class TrapData : ScriptableObject
{
    public float damage;
    public TrapType trapType;
}
public enum TrapType : byte
{
    Spikes,
    Fire,
    Poison
}
