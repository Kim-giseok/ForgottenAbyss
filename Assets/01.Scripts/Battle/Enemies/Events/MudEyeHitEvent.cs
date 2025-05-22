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

        var health = controller.resourceHandler.Get(EnemyStatType.Health);
        UIManager.Instance.BossHealthUI.SetPercentage((int)(health.value / health.maxValue * 100));
        
        if (controller.resourceHandler.Get(EnemyStatType.Health).value <= 0)
        {
            enemyPool.SetActive(false);
            BoltsPool.Instance.Clear();
            
            SoundManager.Instance.Playsfx("BossDeath");
            SoundManager.Instance.FadeOutBGM();
            UIManager.Instance.BossHealthUI.Active(false);
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