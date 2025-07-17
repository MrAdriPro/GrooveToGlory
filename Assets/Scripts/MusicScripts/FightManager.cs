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
    [BoxGroup("Combat UI")] public Slider playerHealthBar;
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
        if (combatActive)
        {
            if (player != null && playerHealthBar.maxValue == player.maxHealth)
                playerHealthBar.value = Mathf.Lerp(playerHealthBar.value, player.currentHealth, Time.deltaTime * lerpSpeed);

            if (currentEnemy != null && enemyHealthBar.maxValue == currentEnemy.data.maxHealth)
                enemyHealthBar.value = Mathf.Lerp(enemyHealthBar.value, currentEnemy.currentHealth, Time.deltaTime * lerpSpeed);

            if (!activeSong.isPlaying)
                EndCombat();
        }
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

        playerHealthBar.maxValue = player.maxHealth;
        playerHealthBar.value = player.currentHealth;

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



    public void RegisterNote(Note note) => activeNotes.Add(note);
    public void UnregisterNote(Note note) => activeNotes.Remove(note);
    public int GetCurrentCombo() => currentCombo;
    public int GetMaxCombo() => maxComboReached;

    public void HitNote(Note note)
    {
        if (!note.canBePressed || note.resolved) return;
        if (note.note.isDangerous)
        {
            float damageDone = damageToPlayer * dangerousNoteMultiplier;
            player.TakeDamage(damageDone);
            print($"daño al player de {damageDone}");
            note.resolved = true;
            UnregisterNote(note);
            Destroy(note.gameObject);
            currentCombo = 0;
            return;
        }
        note.resolved = true;
        UnregisterNote(note);
        Destroy(note.gameObject);

        currentCombo++;

        if (currentCombo > maxComboReached)
            maxComboReached = currentCombo;

        float multiplier = 1 + (currentCombo / comboNumberToMultiplyDamage) * damageMultiplierPerCombo;
        float finalDamage = damageToEnemy * multiplier;

        print($"Combo: {currentCombo}, Final Damage: {finalDamage}");
        currentEnemy.TakeDamage(finalDamage);
    }

    public void MissNote(Note note)
    {
        float damage = damageToPlayer;
        if (note != null && note.note.isDangerous)
        {
            return;
        }
        player.TakeDamage(damageToPlayer);
        currentCombo = 0;
    }
}
