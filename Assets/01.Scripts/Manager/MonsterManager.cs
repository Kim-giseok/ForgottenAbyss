using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterManager : LaberBase
{
    [SerializeField] List<EnemyController> enemies = new();

    public void AddList(EnemyController enemy)
    {
        if (enemies != null && enemies.Count == 0)
        {
            isSwitched = true;
            DisSwitchMachine();
        }
        enemies.Add(enemy);
    }

    public void RemoveEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
        enemy.gameObject.SetActive(false);
        if (enemies.Count <= 0)
            EnemyCleared();
    }

    void EnemyCleared()
    {
        SwitchMachine();
    }
}
