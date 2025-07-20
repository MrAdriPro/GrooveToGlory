using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using System.Collections;

public class FightManager : MonoBehaviour
{
    public static FightManager instance;
    private bool combatHasStarted = false;

    public PlayerHealth player;
    public EnemyHealth currentEnemy;
    private Player_Movement playerMovement;
    public NoteSpawner noteSpawner;


    public float damageToEnemy = 10f;
    public float damageToPlayer = 10f;

    [Header("Combat UI")]
    [BoxGroup("Combat UI")] public GameObject combatUI;
    [BoxGroup("Combat UI")] public Camera combatCamera;
    [BoxGroup("Combat UI")] public GameObject playerPrefab;
    [BoxGroup("Combat UI")] public GameObject enemyPrefab;
    [BoxGroup("Combat UI")] public Image enemyImage;
    [BoxGroup("Combat UI")] public Image playerImage;
    [BoxGroup("Combat UI")] public SpriteRenderer playerPortrait;
    [BoxGroup("Combat UI")] public Slider enemyHealthBar;
    [BoxGroup("Combat UI")] public Slider playerCombatHealthBar;
    float originalFOV;

    public float lerpSpeed = 5f;
    public List<Note> activeNotes = new List<Note>();
    private int currentCombo = 0;
    private int maxComboReached = 0;
    public float damageMultiplierPerCombo = 1f;
    [SerializeField] private int comboNumberToMultiplyDamage = 10;
    public float dangerousNoteMultiplier = 100f;

    [Header("Song")]
    private AudioSource activeSong;
    private bool combatActive = false;
    public TextMeshProUGUI countdownText;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        originalFOV = Camera.main.fieldOfView;
        playerMovement = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player_Movement>();

    }

    private void Update()
    {
        if (!combatActive) return;

        UpdateHealthBars();
        if (!activeSong.isPlaying)
            EndCombat();
    }
    void UpdateHealthBars()
    {
        playerCombatHealthBar.value = Mathf.Lerp(playerCombatHealthBar.value, player.currentHealth, Time.deltaTime * lerpSpeed);
        GameController.instance.worldHealthBar.value = Mathf.Lerp(GameController.instance.worldHealthBar.value, player.currentHealth, Time.deltaTime * lerpSpeed);

        if (enemyHealthBar.maxValue == currentEnemy.data.maxHealth)
            enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, currentEnemy.currentHealth, Time.deltaTime * lerpSpeed);
    }





    public void StartCombat(EnemyData enemyData)
    {
        if (combatHasStarted) return;

        playerPrefab.SetActive(false);
        enemyPrefab.SetActive(false);

        currentEnemy.SetEnemyData(enemyData);
        noteSpawner.enemyData = enemyData;


        combatUI.SetActive(true);
        combatCamera.gameObject.SetActive(true);
        noteSpawner.gameObject.SetActive(true);
        combatActive = true;

        float startingHealth = GameController.instance.worldHealthBar.value;
        player.currentHealth = startingHealth;
        playerCombatHealthBar.maxValue = player.maxHealth;
        playerCombatHealthBar.value = player.currentHealth;

        enemyHealthBar.maxValue = enemyData.maxHealth;
        enemyHealthBar.value = enemyData.maxHealth;

        playerImage.sprite = playerPortrait.sprite;
        enemyImage.sprite = enemyData.portrait;

        currentCombo = 0;
        maxComboReached = 0;
        combatHasStarted = true;

        StartCoroutine(CountdownBeforeCombat());
    }

    public IEnumerator ZoomEffectBeforeCombat(float zoomOutFOV, float zoomInFOV, float duration, EnemyData enemyData)
    {
        Camera cam = Camera.main;
        float timer = 0f;


        while (timer < duration / 2)
        {
            cam.fieldOfView = Mathf.Lerp(originalFOV, zoomOutFOV, timer / (duration / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = zoomOutFOV;
        timer = 0f;

        while (timer < duration / 2)
        {
            cam.fieldOfView = Mathf.Lerp(zoomOutFOV, zoomInFOV, timer / (duration / 2));
            timer += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = zoomInFOV;
        cam.fieldOfView = originalFOV;

        StartCombat(enemyData);
    }

    private IEnumerator CountdownBeforeCombat()
    {
        combatActive = false;
        int countdown = 3;
        countdownText.gameObject.SetActive(true);

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        countdownText.text = "¡GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        noteSpawner.PlayRandomSong();
        noteSpawner.activeSong.Play();
        noteSpawner.canSpawnNotes = true;

        activeSong = noteSpawner.activeSong;
        combatActive = true;
    }

    public void EndCombat()
    {
        if (activeSong != null && activeSong.isPlaying)
            activeSong.Stop();

        playerMovement.enabled = true;

        noteSpawner.canSpawnNotes = false;
        noteSpawner.songTime = 0f;
        noteSpawner.beatTimer = 0f;

        noteSpawner.gameObject.SetActive(false);
        combatCamera.gameObject.SetActive(false);
        combatUI.SetActive(false);
        playerPrefab.SetActive(true);

        noteSpawner.slimeOverlay.SetActive(false);
        combatActive = false;
        combatHasStarted = false;
    }



    /// <summary>
    /// Adds a note to the list of currently active notes in the fight.
    /// </summary>
    public void RegisterNote(Note note) => activeNotes.Add(note);

    /// <summary>
    /// Removes a note from the list of active notes.
    /// Called when a note is hit or missed and should no longer be interactable.
    /// </summary>
    public void UnregisterNote(Note note) => activeNotes.Remove(note);

    /// <summary>
    /// Returns the current combo count (number of consecutive successful hits).
    /// </summary>
    public int GetCurrentCombo() => currentCombo;

    /// <summary>
    /// Returns the highest combo reached during the current fight.
    /// </summary>
    public int GetMaxCombo() => maxComboReached;

    /// <summary>
    /// Returns true if combat is currently active.
    /// </summary>
    public bool IsCombatActive() => combatActive;

    /// <summary>
    /// Called when the player successfully hits a note.
    /// Handles dangerous notes, combo logic, and damage calculation.
    /// </summary>
    public void HitNote(Note note)
    {
        // Ignore if combat is not active
        if (!combatActive) return;
        // Ignore if note can't be pressed or was already resolved
        if (!note.canBePressed || note.resolved) return;

        // If the note is dangerous, deal heavy damage to player and reset combo
        if (note.note.isDangerous)
        {
            float damageDone = damageToPlayer * dangerousNoteMultiplier;
            player.TakeDamage(damageDone);
            print($"daño al player de {damageDone}");
            currentCombo = 0;
            Destroy(note.gameObject);
            UnregisterNote(note);
            return;
        }

        // Mark note as resolved and remove from active notes
        note.resolved = true;
        UnregisterNote(note);
        Destroy(note.gameObject);

        // Increase combo count
        currentCombo++;

        // Update max combo if needed
        if (currentCombo > maxComboReached)
            maxComboReached = currentCombo;

        // Calculate damage with combo multiplier
        float multiplier = 1 + (currentCombo / comboNumberToMultiplyDamage) * damageMultiplierPerCombo;
        float finalDamage = damageToEnemy * multiplier;

        print($"Combo: {currentCombo}, Final Damage: {finalDamage}");
        currentEnemy.TakeDamage(finalDamage);
    }

    /// <summary>
    /// Called when the player misses a note.
    /// If the note is dangerous, missing does nothing (handled in HitNote).
    /// Otherwise, deals damage to the player and resets combo.
    /// </summary>
    public void MissNote(Note note)
    {
        // Ignore if combat is not active
        if(!combatActive) return;
        // Missing a dangerous note does nothing
        if (note != null && note.note.isDangerous)
            return;

        // Deal damage to player and reset combo
        player.TakeDamage(damageToPlayer);
        currentCombo = 0;
    }
}
