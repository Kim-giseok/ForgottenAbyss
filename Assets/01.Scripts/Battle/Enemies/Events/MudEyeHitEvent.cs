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
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void GetDamage(float damage)
    {
        controller.GetDamage(damage);

        var health = controller.resourceHandler.Get(EnemyStatType.Health);
        UIManager.Instance.BossHealthUI.SetPercentage((int)(health.value / health.maxValue * 100));
        
        if (controller.resourceHandler.Get(EnemyStatType.Health).value <= 0)
        {
            // 비용 문제 추후 생각해보기
            foreach (Transform child in enemyPool.gameObject.transform)
            {
                child.GetComponent<EnemyController>().Die();
            }
            BoltsPool.Instance.Clear();
            
            transform.position = Vector3.zero;

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