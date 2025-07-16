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
    private EnemyRandomMovement enemyRandomMovement;

    public float damageToEnemy = 10f;
    public float damageToPlayer = 10f;

    [Header("Combat UI")]
    [BoxGroup("Combat UI")] public GameObject combatUI;
    [BoxGroup("Combat UI")] public Camera combatCamera;
    [BoxGroup("Combat UI")] public GameObject playerPrefab;
    [BoxGroup("Combat UI")] public GameObject enemyPrefab;
    [BoxGroup("Combat UI")] public NoteSpawner noteSpawner;
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

    [Header("Song")]
    private AudioSource activeSong;
    private bool combatActive = false;
    public TextMeshProUGUI countdownText;

    [Header("Pixel Art Camera")]
    public Camera pixelCamera;
    public RenderTexture pixelRenderTexture;
    public Vector2Int pixelResolution = new Vector2Int(320, 180);
    public Vector2Int fullResolution = new Vector2Int(1920, 1080);

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
        playerMovement = FindFirstObjectByType<Player_Movement>();
        enemyRandomMovement = FindFirstObjectByType<EnemyRandomMovement>();


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

        EnablePixelCamera(true);

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

        Debug.Log("Iniciando Zoom");

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

        EnablePixelCamera(false); // VOLVER A LA CÁMARA NORMAL

        combatActive = false;
        combatHasStarted = false;
    }

    private void EnablePixelCamera(bool enable)
    {
        if (pixelCamera == null || pixelRenderTexture == null)
        {
            Debug.LogError("PixelCamera o PixelRenderTexture no están asignados en el Inspector.");
            return;
        }

        // Activar/desactivar las cámaras de forma segura
        pixelCamera.gameObject.SetActive(enable);

        if (Camera.main != null)
        {
            Camera.main.enabled = !enable; // NO desactivar el GameObject, solo la cámara
        }

        if (enable)
        {
            pixelRenderTexture.Release();
            pixelRenderTexture.width = 320;
            pixelRenderTexture.height = 180;
            pixelRenderTexture.Create();

            pixelCamera.targetTexture = pixelRenderTexture;
        }
    }


    public void RegisterNote(Note note) => activeNotes.Add(note);
    public void UnregisterNote(Note note) => activeNotes.Remove(note);
    public int GetCurrentCombo() => currentCombo;
    public int GetMaxCombo() => maxComboReached;

    public void HitNote(Note note)
    {
        if (!note.canBePressed || note.resolved) return;

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

    public void MissNote()
    {
        player.TakeDamage(damageToPlayer);
        if (currentCombo > 0)
            currentCombo = 0;
    }
}
