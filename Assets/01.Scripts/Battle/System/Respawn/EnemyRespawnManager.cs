using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// 동적으로 몬스터를 소환해야하는 경우가 발생
public class EnemyRespawnManager: Singleton<EnemyRespawnManager>
{
    [Serializable] public class Enemy { public Enemies.Enemy enemyName; public GameObject enemyPrefab; }
    [SerializeField] private List<Enemy> enemyList = new();
    private readonly Dictionary<Enemies.Enemy, GameObject> _enemyList = new();

    private void Awake()
    {
        foreach (var mapping in enemyList)
        {
            _enemyList[mapping.enemyName] = mapping.enemyPrefab;
        }
    }
    
    public GameObject Generate(Enemies.Enemy enemy, Vector2 position)
    {
        GameObject instance = Instantiate(_enemyList[enemy]);
        instance.transform.position = position;
        return instance;
    }
}