using System;
using System.Collections.Generic;
using UnityEngine;

// 동적으로 몬스터를 소환해야하는 경우가 발생
public class EnemyRespawner: SingletonLoadRemain<EnemyRespawner>
{
    [Serializable] public class Enemy { public global::Enemy enemyName; public GameObject enemyPrefab; }
    [SerializeField] private List<Enemy> enemyList = new();
    private readonly Dictionary<int, GameObject> _enemyList = new();

    protected override void Awake()
    {
        base.Awake();

        foreach (var mapping in enemyList)
        {
            _enemyList[(int)mapping.enemyName] = mapping.enemyPrefab;
        }
    }
    
    public GameObject Create(global::Enemy enemy, Vector2 position)
    {
        GameObject instance = Instantiate(_enemyList[(int)enemy]);
        instance.transform.position = position;
        return instance;
    }
}