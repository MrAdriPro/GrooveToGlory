using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemy", order = 1)]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public float maxHealth = 100f;
    public Sprite portrait; 
}
