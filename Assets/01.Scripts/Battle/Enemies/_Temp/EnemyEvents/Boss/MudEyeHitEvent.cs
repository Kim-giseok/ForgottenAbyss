using UnityEngine;

public class MudEyeHitEvent: MonoBehaviour,IDamagable
{
    private EnemyController controller;

    private float totalDamage = 0;
    public float maxDamage;
    
    public MapSwapper mapSwapper;
    public GameObject enemyPool;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    public void GetDamage(float damage)
    {
        controller.GetDamage(damage);
        if (controller.resourceHandler.Get(EnemyStatType.Health).value - damage <= 0)
        {
            enemyPool.SetActive(false);
            BoltsPool.Instance.Clear();
            return;
        }
        
        totalDamage += damage;
        if (totalDamage > maxDamage)
        {
            totalDamage = 0;
            mapSwapper.SwapMapAsync().Forget();
        }
    }
}