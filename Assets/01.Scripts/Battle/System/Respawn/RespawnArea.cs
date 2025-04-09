using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    public Vector2 respawnArea;
    public bool isRespawnOnStart;
    
    public List<GameObject> enemyList; 
    private List<GameObject> currEnemies;
    
    public int maxCount;
    private int currCount = 0;
    
    public float cooldown;

    public void SpawnEnemies()
    {
        for (int index = 0; index < maxCount; index++)
        {
            GameObject enemy = Instantiate(enemyList[Random.Range(0, enemyList.Count)], new Vector2(transform.position.x + Random.Range(- (respawnArea.x / 2), respawnArea.x / 2), transform.position.y + Random.Range(- (respawnArea.y / 2), respawnArea.y / 2)), Quaternion.identity);
            enemy.transform.SetParent(transform);
        }
    }

    public void DestroyEnemy(GameObject selectedEnemy)
    {
        var existEnemy = currEnemies.Find(enemy => enemy == selectedEnemy);
        if (!existEnemy)
        {
            Debug.LogWarning("respawnArea: enemy is not in area");
            return;
        }

        currEnemies.Remove(existEnemy);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, respawnArea);
    }

    private void Start()
    {
        if (isRespawnOnStart)
        {
            SpawnEnemies();
        }
    }
}
