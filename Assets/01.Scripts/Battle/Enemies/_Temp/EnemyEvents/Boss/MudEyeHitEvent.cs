using UnityEngine;

public class MudEyeHitEvent: MonoBehaviour,IDamagable
{
    private EnemyController controller;

    private float totalDamage = 0;
    public float maxDamage;
    public MapSwapper mapSwapper;

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    public void GetDamage(float damage)
    {
        controller.GetDamage(damage);
        
        totalDamage += damage;
        if (totalDamage > maxDamage)
        {
            totalDamage = 0;
            mapSwapper.SwapMapAsync().Forget();
        }
    }
}