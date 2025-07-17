using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public enum Direction { Left, Right, Up, Down }

    [Header("Note Prefabs")]
    public GameObject[] notePrefabs = new GameObject[4]; // Array to hold different note prefabs
    public GameObject boneNotePrefab; 

    [Header("Spawn Points")]
    public Transform[] spawnPoints = new Transform[4]; // Array to hold different spawn points for notes

    [Header("BPM")]
    public float bpm = 120f;
    public SongData songData;

    [Header("Spectrum Thresholds")]
    [Tooltip("Thresholds for low, mid, and high frequencies")]
    [Range(0, 1)] public float[] thresholds = new float[3] { 0.1f, 0.07f, 0.05f }; // Thresholds for low, mid, and high frequencies

    [Header("Double Notes Settings")]
    [Range(0, 1)] public float chanceToSpawnDoubleNote = 0.2f; // Chance to spawn a double note

    [Header("Breaking Song Settings")]
    public bool breakingSong = false;
    public float songTime = 0f;
    [Header("beat settings")]
    private float[] spectrum = new float[256];
    public AudioSource activeSong;

    public float beatTimer = 0f;
    private float secondsPerBeat;
    [HideInInspector]
    public bool canSpawnNotes = false;
    [Header("References")]
    public EnemyData enemyData;
    [Header("Special attack settings")]
    private float specialAttackTimer = 0f;
    public float specialAttackInterval = 5f;
    public GameObject slimeOverlay;
    private PlayerHealth player;
    public CanvasGroup slimeCanvasGroup;
    private bool skipNextNote = false;
    public EnemyAttackManager enemyAttackManager;


    void Start()
    {
        enemyAttackManager.Initialize(this);
        player = FindFirstObjectByType<PlayerHealth>();
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        //meterlo en voids 
        if (!canSpawnNotes || activeSong == null) return;

        songTime += Time.deltaTime;
        specialAttackTimer += Time.deltaTime;

        if (specialAttackTimer > specialAttackInterval)
        {
            specialAttackTimer = 0;
            enemyAttackManager.TriggerEnemyEffect();
        }

        if (OnBreak(songTime))
        {
            secondsPerBeat = 60f / (songData.bpm * 2); // Adjust the beat tempo for breaking song
            Note_Data.speed = 10;
        }
        else
        {
            secondsPerBeat = 60f / songData.bpm; // Normal beat tempo
            Note_Data.speed = 5;
        }

        beatTimer += Time.deltaTime;

        if (beatTimer >= secondsPerBeat)
        {
            beatTimer -= secondsPerBeat;
            if (OnPause(songTime)) return;
            if (skipNextNote)
            {
                skipNextNote = false;
                return;
            }
            AnalyzeSpectrumAndSpawnNote();
        }
    }
    
    
    public void PlayRandomSong()
    {
        int randomIndex = Random.Range(0, enemyData.songData.Count);
        activeSong.clip = enemyData.songData[randomIndex].song;
        songData = enemyData.songData[randomIndex];
        Debug.Log($"Playing song: {activeSong.clip.name}");

    }

    private bool OnPause(float currentTime)
    {
        foreach (Vector2 period in songData.pausePeriods)
        {
            if (currentTime >= period.x && currentTime <= period.y)
            {
                return true; //is on pause
            }

        }
        return false; //not pause
    }
    private bool OnBreak(float currentTime)
    {
        foreach (Vector2 period in songData.breakingPeriods)
        {
            if (currentTime >= period.x && currentTime <= period.y)
            {
                return true; // period of pause
            }

        }
        return false; // No está en pausa
    }

    void AnalyzeSpectrumAndSpawnNote()
    {
        float random = Random.Range(0, 10); // Randomly choose a note type to spawn
        activeSong.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float low = spectrum[1] + spectrum[2];
        float mid = spectrum[15] + spectrum[20];
        float high = spectrum[50] + spectrum[60];

        bool lowThreshold = low > thresholds[0];

        List<Direction> posibleNotes = new List<Direction>();

        if (low > thresholds[0])
        {
            posibleNotes.Add(Direction.Left);
            posibleNotes.Add(Direction.Down);
        }
        if (mid > thresholds[1])
        {
            posibleNotes.Add(Direction.Down);
            posibleNotes.Add(Direction.Up);
        }
        if (high > thresholds[2])
        {
            posibleNotes.Add(Direction.Up);
            posibleNotes.Add(Direction.Right);
        }

        if (posibleNotes.Count == 0)
        {
            posibleNotes.Add((Direction)Random.Range(0, 4));
        }

        List<Direction> uniqueNotes = new List<Direction>();
        foreach (Direction dir in posibleNotes)
        {
            if (!uniqueNotes.Contains(dir))
            {
                uniqueNotes.Add(dir);
            }
        }

        int notesToSpawn = Random.value < chanceToSpawnDoubleNote && uniqueNotes.Count > 1 ? 2 : 1; // Decide whether to spawn one or two notes

        List<Direction> finalNotes = new List<Direction>();
        while (finalNotes.Count < notesToSpawn)
        {
            Direction note = uniqueNotes[Random.Range(0, uniqueNotes.Count)];
            if (!finalNotes.Contains(note))
            {
                finalNotes.Add(note);
            }
        }
        foreach (Direction note in finalNotes)
        {
            int index = (int)note;
            if (notePrefabs[index] != null && spawnPoints[index] != null)
            {
                Instantiate(notePrefabs[index], spawnPoints[index].position, Quaternion.identity);
            }
        }
        // Debug.Log($"Low: {low}, Mid: {mid}, High: {high}");
    }

    public float RoundToOneDecimal(float number) => (float)System.Math.Round(number, 1);

}
