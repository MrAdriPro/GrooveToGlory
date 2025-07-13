using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public float currentHealth;

    private void Start()
    {
        if (data != null)
            currentHealth = data.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, data.maxHealth);

        if (currentHealth <= 0)
        {
            Debug.Log($"{data.enemyName} ha sido derrotado.");
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / data.maxHealth;
    }
}
