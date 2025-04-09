using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    public Rect respawnArea;
    
    public List<GameObject> enemyList;
    public List<GameObject> currEnemies;
    public float cooldown;
    
    public int maxCount;
    private int currCount = 0;

    public void SpawnEnemies()
    {
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
        Gizmos.DrawWireCube(respawnArea.center, respawnArea.size);
    }
}
