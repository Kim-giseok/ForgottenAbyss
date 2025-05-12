using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/StatSO")]
public class EnemyStatSO: ScriptableObject
{
    public string enemyName;
    
    public float health;
    public float mana;
    
    public float attack;
    public float speed;
}