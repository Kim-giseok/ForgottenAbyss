using UnityEngine;

[CreateAssetMenu(menuName = "SO/Enemy/StatsSO")]
public class EnemyStatsSO: ScriptableObject
{
    public float health;
    public float mana;
    
    public float attack;
    public float speed;
}