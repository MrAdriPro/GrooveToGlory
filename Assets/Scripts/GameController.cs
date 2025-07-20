using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    private PlayerHealth player;

    [Header("UI")]
    public Slider worldHealthBar; 

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<PlayerHealth>();
        worldHealthBar.maxValue = player.maxHealth;
        worldHealthBar.value = worldHealthBar.maxValue;
    }

    public void UpdateWorldHealthBar(float newHealth)
    {
        if (worldHealthBar != null)
            worldHealthBar.value = newHealth;
    }
}
