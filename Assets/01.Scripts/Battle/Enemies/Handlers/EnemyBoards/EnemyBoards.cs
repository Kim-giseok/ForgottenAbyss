using System;
using System.Collections.Generic;

public class EnemyBoards
{
    public Dictionary<Enemies.Enemy, Func<EnemyBoard>> boards = new()
    {
        { Enemies.Enemy.NightBone, () => new NightBoneBoard() }
    };

    public EnemyBoard CreateBoard(Enemies.Enemy enemy)
    {
        return boards[enemy]();
    }
}