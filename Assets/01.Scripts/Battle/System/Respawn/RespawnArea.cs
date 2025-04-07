using System.Collections.Generic;
using UnityEngine;

public class RespawnArea : MonoBehaviour
{
    public Rect respawnArea;
    
    public List<GameObject> enemyList;
    
    public int maxCount;
    private int currCount = 0;
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(respawnArea.center, respawnArea.size);
    }
}
