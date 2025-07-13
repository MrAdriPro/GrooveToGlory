using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerHealth player;
    public Enemy currentEnemy;

    public float damageToEnemy = 10f;
    public float damageToPlayer = 10f;

    public Slider playerHealthBar;
    public Slider enemyHealthBar;

    public float lerpSpeed = 5f;

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
        if (player != null)
        {
            float target = player.currentHealth;
            playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, target, Time.deltaTime * lerpSpeed);
        }


        if (currentEnemy != null)
        {
            float target = currentEnemy.currentHealth;
            enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, target, Time.deltaTime * lerpSpeed);


        }
    }

    public void RegisterNote(Note note)
    {
        activeNotes.Add(note);
    }

    public void UnregisterNote(Note note)
    {
        activeNotes.Remove(note);
    }

    public void HitNote(Note note)
    {
        if (!note.canBePressed || note.resolved) return;

        note.resolved = true;
        UnregisterNote(note);
        Destroy(note.gameObject);

        currentEnemy.TakeDamage(damageToEnemy);
    }

    public void MissNote()
    {
        player.TakeDamage(damageToPlayer);
    }
}
