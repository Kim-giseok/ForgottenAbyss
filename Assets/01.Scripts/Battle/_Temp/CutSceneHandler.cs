using UnityEngine;

public class CutSceneHandler: MonoBehaviour
{
    public void OnAgisNarrationEnd()
    {
        Destroy(gameObject);
        EnemyRespawner.Instance.Create(Enemies.Enemy.Agis, transform.position);
    }        
}