using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public EnemyData data;
    public float currentHealth;

    private void Start()
    {
        if (data != null)
            currentHealth = data.maxHealth;
    }
    public void SetEnemyData(EnemyData newData)
    {
        data = newData;
        currentHealth = data.maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, data.maxHealth);

        if (currentHealth <= 0)
        {
            //Aqui imagino que tendremos que llamar a lo de que tire items y tal
            FightManager.instance.EndCombat();
            Debug.Log($"{data.enemyType} defeated!");
        }
    }
    

    public float GetHealthPercent()
    {
        return currentHealth / data.maxHealth;
    }
}
