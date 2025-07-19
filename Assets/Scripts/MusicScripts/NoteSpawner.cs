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
    public CanvasGroup slimeCanvasGroup;
    [HideInInspector]  
    public bool nextNoteIsBone = false;
    public EnemyAttackManager enemyAttackManager;


    void Start()
    {
        enemyAttackManager.Initialize(this);
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {
        //meterlo en voids 
        if (!canSpawnNotes || activeSong == null)
            return;

        songTime += Time.deltaTime;
        specialAttackTimer += Time.deltaTime;

        if (specialAttackTimer > specialAttackInterval)
        {
            specialAttackTimer = 0f;
            enemyAttackManager.TriggerEnemyEffect();
        }

        UpdateBeatTime();
    }


    void UpdateBeatTime()
    {
        float bpmToUse = OnBreak(songTime) ? songData.bpm * 2 : songData.bpm;
        secondsPerBeat = 60f / bpmToUse;
        Note_Data.speed = OnBreak(songTime) ? 10 : 5;

        beatTimer += Time.deltaTime;

        if (beatTimer >= secondsPerBeat)
        {
            beatTimer -= secondsPerBeat;

            if (OnPause(songTime)) return;

            AnalyzeSpectrumAndSpawnNote();
        }
    }

    /// <summary>
    /// Picks a random song from the enemy's song list and sets it as the active song.
    /// Also updates the songData reference for spectrum analysis and timing.
    /// </summary>
    public void PlayRandomSong()
    {
        int randomIndex = Random.Range(0, enemyData.songData.Count);
        activeSong.clip = enemyData.songData[randomIndex].song;
        songData = enemyData.songData[randomIndex];
        Debug.Log($"Playing song: {activeSong.clip.name}");
    }

    /// <summary>
    /// Checks if the current time is within any of the provided time ranges.
    /// Used for pause and break periods in the song.
    /// </summary>
    private bool IsInTimeRange(List<Vector2> ranges, float time)
    {
        foreach (var range in ranges)
            if (time >= range.x && time <= range.y)
                return true;

        return false;
    }

    /// <summary>
    /// Returns true if the current song time is within a pause period.
    /// Notes should not spawn during pauses.
    /// </summary>
    private bool OnPause(float time) => IsInTimeRange(songData.pausePeriods, time);

    /// <summary>
    /// Returns true if the current song time is within a break period.
    /// Breaks can affect BPM and note speed.
    /// </summary>
    private bool OnBreak(float time) => IsInTimeRange(songData.breakingPeriods, time);

    /// <summary>
    /// Analyzes the audio spectrum and spawns notes based on frequency thresholds.
    /// Picks note directions based on low/mid/high frequency energy.
    /// Can spawn double notes if conditions are met.
    /// Handles dangerous notes.
    /// </summary>
    void AnalyzeSpectrumAndSpawnNote()
    {
        float random = Random.Range(0, 10); // Used for random note selection
        activeSong.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // Analyze frequency bands
        float low = spectrum[1] + spectrum[2];
        float mid = spectrum[15] + spectrum[20];
        float high = spectrum[50] + spectrum[60];

        // Build list of possible note directions based on thresholds
        List<Direction> availableNotes = new List<Direction>();
        if (low > thresholds[0])
        {
            availableNotes.Add(Direction.Left);
            availableNotes.Add(Direction.Down);
        }
        if (mid > thresholds[1])
        {
            availableNotes.Add(Direction.Down);
            availableNotes.Add(Direction.Up);
        }
        if (high > thresholds[2])
        {
            availableNotes.Add(Direction.Up);
            availableNotes.Add(Direction.Right);
        }

        // If no frequency is strong enough, pick a random direction
        if (availableNotes.Count == 0)
        {
            availableNotes.Add((Direction)Random.Range(0, 4));
        }

        // Decide whether to spawn one or two notes
        Direction[] notePool = new List<Direction>(availableNotes).ToArray();
        int notesToSpawn = Random.value < chanceToSpawnDoubleNote && notePool.Length > 1 ? 2 : 1;

        for (int i = 0; i < notesToSpawn; i++)
        {
            Direction note = notePool[Random.Range(0, notePool.Length)];
            int index = (int)note;
            if (notePrefabs[index] != null && spawnPoints[index] != null)
            {
                // Use boneNotePrefab if nextNoteIsBone is set, otherwise use normal prefab
                GameObject prefabToUse = nextNoteIsBone ? boneNotePrefab : notePrefabs[index];
                GameObject noteGO = Instantiate(prefabToUse, spawnPoints[index].position, Quaternion.identity);

                // Set note properties (dangerous, direction)
                if (noteGO.TryGetComponent<Note>(out var noteComponent))
                {
                    if(nextNoteIsBone)
                        noteComponent.note.isDangerous = true;

                    switch (index)
                    {
                        case 0:
                            noteComponent.note.direction = NoteDirection.Down;
                            break;
                        case 1:
                            noteComponent.note.direction = NoteDirection.Up;
                            break;
                        case 2:
                            noteComponent.note.direction = NoteDirection.Left;
                            break;
                        case 3:
                            noteComponent.note.direction = NoteDirection.Right;
                            break;
                    }
                }

                nextNoteIsBone = false;
            }
        }
    }

  
    public float RoundToOneDecimal(float number) => (float)System.Math.Round(number, 1);

}
