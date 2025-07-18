using UnityEngine;


[CreateAssetMenu(menuName = "Traps/New Trap")]
public class TrapData : ScriptableObject
{
    public float damage;
    public TrapType trapType;
}
public enum TrapType : byte
{
    Spikes
}
