using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemys/New Enemy")]
public class EnemyData : ScriptableObject
{
    public enum EnemyType{Skeleton, Slime, Zombie }
    public EnemyType enemyType;
    public float maxHealth = 100f;
    public List<SongData> songData;
    public Sprite portrait;
    public Sprite zoomedSprite;
}



