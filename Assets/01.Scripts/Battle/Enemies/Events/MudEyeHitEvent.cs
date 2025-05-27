using System.Collections;
using UnityEngine;

public class MudEyeHitEvent: MonoBehaviour,IDamagable
{
    private EnemyController controller;

    private float totalDamage = 0;
    public float maxDamage;
    
    public MapSwapper mapSwapper;
    public GameObject enemyPool;

    public bool isEnd;
    
    // 변경 주기를 통해 과도한 변경 발생하는 점 수정
    public float swapCooldown = 3f;
    private bool canSwap = true;
    
    private void Awake()
    {
        controller = GetComponent<EnemyController>();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    public void GetDamage(float damage)
    {
        if (isEnd) return;
        
        controller.GetDamage(damage);

        var health = controller.Resource.Get(EnemyStatType.Health);
        UIManager.Instance.BossHealthUI.SetPercentage((int)(health.value / health.maxValue * 100));
        
        if (controller.Resource.Get(EnemyStatType.Health).value <= 0)
        {
            isEnd = true;
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
        if (totalDamage > maxDamage && canSwap)
        {
            totalDamage = 0;
            mapSwapper.SwapMapAsync().Forget();
            
            canSwap = false;
            StartCoroutine(SwapCooldownRoutine());
        }
    }

    private IEnumerator SwapCooldownRoutine()
    {
        yield return new WaitForSeconds(swapCooldown);
        canSwap = true;
    }
}