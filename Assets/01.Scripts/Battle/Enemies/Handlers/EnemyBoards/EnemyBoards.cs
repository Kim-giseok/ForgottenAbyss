using System;
using System.Collections.Generic;

public class EnemyBoards
{
    public EnemyBoard Create(Enemies.Enemy enemy)
    {
        return enemy switch
        {
            Enemies.Enemy.NightBone => new NightBoneBoard(),
            _ => throw new ArgumentException($"Board for {enemy} is not defined.")
        };
    }
}