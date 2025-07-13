using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public HealthSystem playerHealth;
    public HealthSystem enemyHealth;

    public float damageToEnemy = 10f;
    public float damageToPlayer = 10f;

    public Slider playerHealthBar;
    public Slider enemyHealthBar;

    public List<Note> activeNotes = new List<Note>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        UpdateHealthBars();
    }
    private void Start()
    {
        playerHealthBar.maxValue = playerHealth.maxHealth;
        playerHealthBar.value = playerHealth.currentHealth;

        enemyHealthBar.maxValue = enemyHealth.maxHealth;
        enemyHealthBar.value = enemyHealth.currentHealth;
        
    }
    

    void UpdateHealthBars()
    {
        playerHealthBar.value = playerHealth.currentHealth;
        enemyHealthBar.value = enemyHealth.currentHealth;
    }

    public void RegisterNote(Note note)
    {
        if (!activeNotes.Contains(note))
            activeNotes.Add(note);
    }

    public void UnregisterNote(Note note)
    {
        if (activeNotes.Contains(note))
            activeNotes.Remove(note);
    }

    public void HitNote(Note note)
    {
        if (!note.canBePressed) return;

        UnregisterNote(note);
        Destroy(note.gameObject);
        enemyHealth.TakeDamage(damageToEnemy);
    }

    public void MissNote()
    {
        playerHealth.TakeDamage(damageToPlayer);
    }
}
