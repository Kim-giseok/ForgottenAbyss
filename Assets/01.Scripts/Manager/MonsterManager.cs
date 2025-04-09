using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : LaberBase
{
    [SerializeField] List<EnemyController> enemies = new();

    public void AddList(EnemyController enemy)
    {
        DisSwitchMachine();
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
        if (enemies.Count <= 0)
            EnemyCleared();
    }

    void EnemyCleared()
    {
        SwitchMachine();
    }
}
