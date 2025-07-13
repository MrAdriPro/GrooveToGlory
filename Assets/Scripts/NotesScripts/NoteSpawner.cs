using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public enum Direction { Left, Right, Up, Down }

    [Header("Note Prefabs")]
    public GameObject[] notePrefabs = new GameObject[4]; // Array to hold different note prefabs

    [Header("Spawn Points")]
    public Transform[] spawnPoints = new Transform[4]; // Array to hold different spawn points for notes

    [Header("Audio & BPM")]
    public AudioSource audioSource;
    public float bpm = 120f;

    [Header("Spectrum Thresholds")]
    [Tooltip("Thresholds for low, mid, and high frequencies")]
    [Range(0, 1)] public float[] thresholds = new float[3] { 0.1f, 0.07f, 0.05f }; // Thresholds for low, mid, and high frequencies

    [Header("Double Notes Settings")]
    [Range(0, 1)] public float chanceToSpawnDoubleNote = 0.2f; // Chance to spawn a double note

    [Header("Breaking Song Settings")]
    public bool breakingSong = false;
    public List<Vector2> breakingPeriods = new List<Vector2>();
    public List<Vector2> pausePeriods = new List<Vector2>();

    private bool isInPause = false;
    private bool wasInPause = false;
    private float songTime = 0f;
    [Header("beat settings")]
    private float[] spectrum = new float[256];
    private float beatTimer = 0f;
    private float secondsPerBeat;


    void Start()
    {
        secondsPerBeat = 60f / bpm;
    }

    void Update()
    {

        //to do si el tiempo es mayor o igual que la pausa se pausa si el teimpo es menor o giaul se deitne la puase 
        //si hay una pausa activa y va a ocurrir un break se quita la pausa


        songTime += Time.deltaTime;

        // float beatTempoCalculated = RoundToOneDecimal(RoundToOneDecimal(secondsPerBeat) / (RoundToOneDecimal(secondsPerBeat) / 2));

        if (OnBreak(songTime))
        {
            secondsPerBeat = 60f / (bpm * 2); // Adjust the beat tempo for breaking song
            Note_Data.speed = 10;
        }
        else
        {
            secondsPerBeat = 60f / bpm; // Normal beat tempo
            Note_Data.speed = 5;

        }

        beatTimer += Time.deltaTime;

        if (beatTimer >= secondsPerBeat)
        {
            beatTimer -= secondsPerBeat;
            if (OnPause(songTime)) return; // If in pause, skip spawning notes
            AnalyzeSpectrumAndSpawnNote();

        }


    }

    private bool OnPause(float currentTime)
    {
        foreach (Vector2 period in pausePeriods)
        {
            if (currentTime >= period.x && currentTime <= period.y)
            {
                return true; // Está en un periodo de pausa
            }

        }
        return false; // No está en pausa
    }
    private bool OnBreak(float currentTime)
    {
        foreach (Vector2 period in breakingPeriods)
        {
            if (currentTime >= period.x && currentTime <= period.y)
            {
                return true; // Está en un periodo de pausa
            }

        }
        return false; // No está en pausa
    }

    void AnalyzeSpectrumAndSpawnNote()
    {
        float random = Random.Range(0, 10); // Randomly choose a note type to spawn
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        float low = spectrum[1] + spectrum[2];
        float mid = spectrum[15] + spectrum[20];
        float high = spectrum[50] + spectrum[60];

        bool lowThreshold = low > thresholds[0];

        List<Direction> posibleNotes = new List<Direction>();

        // Puedes ajustar el orden o la asignación como quieras
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
