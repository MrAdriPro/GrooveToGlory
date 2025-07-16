using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Song", menuName = "Songs / New Song")]
public class SongData : ScriptableObject
{
    public AudioClip song;
    public float bpm = 120f;
    public List<Vector2> breakingPeriods = new List<Vector2>();
    public List<Vector2> pausePeriods = new List<Vector2>();
}