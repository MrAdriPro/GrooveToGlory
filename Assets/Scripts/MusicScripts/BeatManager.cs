using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class BeatManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] public float _bpm;
    [SerializeField] private AudioSource _audioSource;

    [SerializeField] private Intervals[] _intervals;

    private NoteSpawner noteSpawner;

    private void Start()
    {
        noteSpawner = FindFirstObjectByType<NoteSpawner>();
    }

    private void Update()
    {
        if (_audioSource.clip is not null)
        {
            _bpm = noteSpawner.songData.bpm;

            // Para cada intervalo se comprueba el tiempo en base al BPM
            foreach (Intervals interval in _intervals)
            {

                float sampledTime = (_audioSource.timeSamples / (_audioSource.clip.frequency * interval.GetIntervalLenght(_bpm)));
                interval.CheckForNewInterval(sampledTime);
            }
        }
    }
}

[System.Serializable]
public class Intervals
{
    [SerializeField] private float _steps;
    [SerializeField] private UnityEvent _trigger;

    private int _lastInterval;

    // Comprueba la longitud del intervalo dependiendo del BPM
    public float GetIntervalLenght(float bpm)
    {
        return 60f / (bpm * _steps);
    }

    // Comprueba el proximo BPM
    public void CheckForNewInterval(float interval)
    {
        if (Mathf.FloorToInt(interval) != _lastInterval)
        {
            _lastInterval = Mathf.FloorToInt(interval);
            _trigger.Invoke();
        }
    }
}
