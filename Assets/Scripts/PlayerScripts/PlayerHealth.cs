using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float currentHealth;

    private float targetHealth;
    public float lerpSpeed = 5f;

    private Slider worldHealthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        targetHealth = currentHealth;

        if (GameController.instance != null)
            worldHealthBar = GameController.instance.worldHealthBar;

        if (worldHealthBar != null)
        {
            worldHealthBar.maxValue = maxHealth;
            worldHealthBar.value = currentHealth;
        }
    }

    private void Update()
    {
        if (worldHealthBar != null)
        {
            worldHealthBar.value = Mathf.Lerp(worldHealthBar.value, targetHealth, Time.deltaTime * lerpSpeed);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        targetHealth = currentHealth;

        if (currentHealth <= 0)
        {
            Debug.Log("Jugador muerto");
        }
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}
