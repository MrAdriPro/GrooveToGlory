using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemys/New Enemy")]
public class EnemyData : ScriptableObject
{   
    public enum EnemyType : byte {Skeleton, Slime, Zombie }
    public EnemyType enemyType;
    public float maxHealth = 100f;
    public List<SongData> songData;
    public Sprite portrait;
    public Sprite zoomedSprite;
    [ShowIf("IsZombie")]
    public int[] speedOptions = { 10, 8, 13 };
    #pragma warning disable
    private bool IsZombie => enemyType.Equals(EnemyType.Zombie);
    #pragma warning restore
}



