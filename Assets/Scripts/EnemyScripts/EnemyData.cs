using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth = 100f;
    public Sprite portrait; 
}
