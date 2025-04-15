using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager: Singleton<EnemyRespawnManager>
{
    [SerializeField] public class Enemy { public Enemies.Enemy enemyName; public GameObject instance; }
    public List<Enemy> enemies;

    public void Generate(Enemies.Enemy enemy, Vector2 position)
    {
        GameObject instance = Instantiate(enemies.Find(ememy => ememy.enemyName == Enemies.Enemy.NightBone).instance);
        instance.transform.position = position;
    }

}