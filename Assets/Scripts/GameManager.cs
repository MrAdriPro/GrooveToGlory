using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    // Static instance for Singleton pattern
    public static GameManager instance;

    // References to the player and the current enemy
    public PlayerHealth player;
    public Enemy currentEnemy;

    // Damage dealt to the enemy and the player
    public float damageToEnemy = 10f;
    public float damageToPlayer = 10f;

    // Health bars for player and enemy (UI)
    public Slider enemyHealthBar;
    public Slider playerHealthBar;

    public float lerpSpeed = 5f;

    // List of active notes in the game
    public List<Note> activeNotes = new List<Note>();

    private int currentCombo = 0;
    private int maxComboReached = 0;
    public float damageMultiplierPerCombo = 1f;
    [SerializeField] private int comboNumberToMultiplyDamage = 10;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        // Initialize player health bar
        if (player != null)
        {
            playerHealthBar.maxValue = player.maxHealth;
            playerHealthBar.value = player.currentHealth;
        }

        // Initialize enemy health bar
        if (currentEnemy != null)
        {
            enemyHealthBar.maxValue = currentEnemy.data.maxHealth;
            enemyHealthBar.value = currentEnemy.currentHealth;
        }
    }

    private void Update()
    {
        // Smoothly updates the player's health bar
        if (player != null && playerHealthBar.maxValue == player.maxHealth)
        {
            float target = player.currentHealth;
            playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, target, Time.deltaTime * lerpSpeed);
        }

        // Smoothly updates the enemy's health bar
        if (currentEnemy != null && enemyHealthBar.maxValue == currentEnemy.data.maxHealth)
        {
            float target = currentEnemy.currentHealth;
            enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, target, Time.deltaTime * lerpSpeed);
        }
    }

    // Registers a new active note
    public void RegisterNote(Note note)
    {
        activeNotes.Add(note);
    }

    // Unregisters an active note
    public void UnregisterNote(Note note)
    {
        activeNotes.Remove(note);
    }

    // Method called when a note is hit
    public void HitNote(Note note)
    {
        // If the note can't be pressed or is already resolved, do nothing
        if (!note.canBePressed || note.resolved) return;

        note.resolved = true; // Mark the note as resolved
        UnregisterNote(note); // Remove from active notes
        Destroy(note.gameObject);
        currentCombo++; // Increase the combo count

        if (currentCombo > maxComboReached)
        {
            maxComboReached = currentCombo; // Update max combo if current exceeds it
        }
        float multiplier = 1 + (currentCombo / comboNumberToMultiplyDamage) * damageMultiplierPerCombo; // Calculate damage multiplier
        float finalDamage = damageToEnemy * multiplier; // Apply multiplier to enemy damage

        print($"Combo: {currentCombo}, Final Damage: {finalDamage}");
        currentEnemy.TakeDamage(finalDamage); // Deal damage to the enemy


    }

    // Method called when a note is missed
    public void MissNote()
    {
        player.TakeDamage(damageToPlayer); // Deal damage to the player
        if (currentCombo > 0)
        {
            currentCombo = 0;
        }
    }
    /// <summary>
    /// This is if we want to put it on a UI element to show the current combo jiji xd
    /// </summary>
    /// <returns></returns>
    public int GetCurrentCombo()
    {
        return currentCombo;
    }

    public int GetMaxCombo()
    {
        return maxComboReached;
    }
}
