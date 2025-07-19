using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapData data; // Data describing the trap's properties
    public ScriptableObject trapEffect; // Reference to the ScriptableObject implementing ITrapEffect

    private void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.PLAYER_BODY_TAG))
        {
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();

            // If valid, activate the trap effect using the interface
            if (playerHealth != null && trapEffect is ITrapEffect effect)
            {
                effect.Activate(playerHealth, data);
            }
        }
    }
}
