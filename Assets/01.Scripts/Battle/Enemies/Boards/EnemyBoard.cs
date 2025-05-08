using UnityEngine;

public class EnemyBoard
{
    public Vector2 currMoveDirection = Vector2.zero;
    public float currTime = 0f;

    public int currAttackTick = -1;
    public int lastAttackTick = -1;
}