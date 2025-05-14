using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 동적으로 몬스터를 소환해야하는 경우가 발생
public class EnemiesPool: SingletonLoadRemain<EnemiesPool>
{
    public EnemyController enemy;
    private readonly List<EnemyController> _currEnemies = new();
    
    public void Create( Enemy enemyName, Vector2 position)
    {
        var currEnemy = _currEnemies.Find(prevEnemy => prevEnemy.enemyName == enemyName && !prevEnemy.gameObject.activeSelf);
        if (!currEnemy)
        {
            currEnemy = Instantiate(enemy, position, Quaternion.identity, transform);
            _currEnemies.Add(currEnemy);
        }
        
        currEnemy.transform.position = position;

        currEnemy.Init(enemyName);
        currEnemy.gameObject.SetActive(true);
    }
}