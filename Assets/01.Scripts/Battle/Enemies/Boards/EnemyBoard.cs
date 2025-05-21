using UnityEngine;

public class EnemyBoard
{
    public bool IsAttacking = false;
    public Vector2 CurrMoveDirection = Vector2.zero;
    public float CurrTime = 0f;

    // notice: Agis 특수 board
    public int CurrAttackTick = -1;
    public int LastAttackTick = -1;
    
    // mud 특수 노드
    public bool IsSpawned = false;
}