using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyController : EnemyBaseController, IDamagable
{
    public bool isBaked = false;
    // status로 관리해야할까?
    public bool isIgnoreHitAnim;
    public Enemy enemyName;
    
    public EnemyResourceHandler resourceHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }
    public EnemyRewardHandler rewardHandler { get; private set; }

    // children 시스템 등록
    // public List<EnemyController> children; 
    
    protected override void Awake()
    {
        base.Awake();
        
        resourceHandler = GetComponent<EnemyResourceHandler>();
        statusHandler = GetComponent<EnemyStatusHandler>();
        rewardHandler = GetComponent<EnemyRewardHandler>();
    }

    // animator 변경 시 첫번 째 스프라이트 렌더러로 등록하기
    public void OnValidate()
    {
        // Addressables.LoadAssetsAsync<RuntimeAnimatorController>("EnemyAnimator", null).Completed += (handle) =>
        // {
        //     var currAnimator = handle.Result.FirstOrDefault(anim => anim.name == enemyName.ToString());
        //     if (!currAnimator) return;
        //    
        //     var animator = GetComponent<Animator>();
        //     animator.runtimeAnimatorController = currAnimator;
        //     
        //     var firstClip = animator.runtimeAnimatorController.animationClips.FirstOrDefault();
        //     if (!firstClip) return;
        //     
        //     var bindings = GetObjectReferenceCurveBindings(firstClip);
        //
        //     foreach (var binding in bindings)
        //     {
        //         var keyframes = GetObjectReferenceCurve(firstClip, binding);
        //         var firstSprite = keyframes.FirstOrDefault().value as Sprite;
        //         if(!firstSprite) continue;
        //         
        //         GetComponent<SpriteRenderer>().sprite = firstSprite;
        //         break;
        //     }
        //     
        //     Addressables.LoadAssetAsync<EnemiesViewInfoSO>("EnemiesViewInfoSO").Completed += (handle) =>
        //     {
        //         var currInfo = handle.Result.EnemyViewInfos.Find(info => info.enemyName == enemyName.ToString());
        //         if (currInfo == null) return;
        //         transform.localScale = new Vector2(currInfo.ratio, currInfo.ratio);
        //         
        //         var currCollider = GetComponent<CapsuleCollider2D>();
        //         currCollider.size = currInfo.size;
        //         currCollider.offset = new Vector2(0, currInfo.size.y / 2);
        //     };
        // };
    }

    #if UNITY_EDITOR
    private async Task WaitForAssetsToLoad() { while (!EnemiesLoader.IsLoaded || !EnemiesAnimator.IsLoaded) { await Task.Yield(); } }
    #endif
    
    public async void Start()
    {
        #if UNITY_EDITOR
        await WaitForAssetsToLoad();
        #endif
        
        if (isBaked)
        {
            // 빌드 타임에서는 비효율적인 액션일 수 있음
            SetConfig(enemyName.ToString()); 
            // 에러처리 필요
            machine.Define(EnemiesBT.Get(enemyName)); // 각 개체별 생성되는 방식
            machine.Start();
        }

        // try { MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this); }
        // catch { Debug.Log("there is no MapspawnManager"); }
    }
    
    public void SetConfig(string newEnemyName)
    {
        resourceHandler.Define(EnemiesLoader.Get<EnemyStatSO>(newEnemyName));
        
        soundHandler.Define(EnemiesLoader.Get<EnemySoundSO>(newEnemyName));
        rewardHandler.Define(EnemiesLoader.Get<EnemyRewardSO>(newEnemyName));
        
        // 미리 등록 되면 등록할 필요 없는 요소들
        animHandler.SetController(EnemiesAnimator.animators[newEnemyName]);
        
        var info = EnemiesLoader.EnemiesInfoSO.EnemyViewInfos.Find(info => info.enemyName == enemyName.ToString());
        if (info == null) return;
        
        transform.localScale = new Vector2(info.ratio, info.ratio);
        Collider.size = info.size;
        Collider.offset = new Vector2(0, info.size.y / 2);
    }

    public void Init(Enemy newEnemyName)
    {
        enemyName = newEnemyName;
        SetConfig(newEnemyName.ToString());
        machine.Define(EnemiesBT.Get(enemyName));
        machine.Start();
    }

    private void OnHit(float damage, EnemyStatusHandler.HitType hitType = EnemyStatusHandler.HitType.Stun)
    {
        resourceHandler.Modify(EnemyStatType.Health, -damage);
        
        if (hitType == EnemyStatusHandler.HitType.Normal)
        {
            BoltsPool.Instance.CreateParticle(transform, "Hit2")
                .SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
            BoltsPool.Instance.CreateParticle(transform, "Hit3")
                .SetSize(0.8f).SetColor(Color.yellow).SetPosition(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
         
            SoundManager.Instance.Playsfx("HitByBow2");
        }

        if (hitType == EnemyStatusHandler.HitType.Stun)
        {
            BoltsPool.Instance.CreateParticle(transform, "Hit")
                .SetSize(0.8f).SetColor(new Color(0, 0, 0, 0.2f)).SetPosition(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();
            BoltsPool.Instance.CreateParticle(transform, "Hit")
                .SetSize(0.15f).SetColor(Color.yellow).SetPosition(transform.position + new Vector3(Random.Range(-0.2f, 0.2f), 0.6f + Random.Range(-0.2f, 0.2f))).Play();

            SoundManager.Instance.Playsfx("HitByMelee");
        }
        
        if (resourceHandler.Get(EnemyStatType.Health).currValue <= 0)
        {
            statusHandler.SetMode(EnmeyMode.Hit, true);
            machine.Notify();
        }
    }

    public void GetDamageByType(float damage, EnemyStatusHandler.HitType hitType = EnemyStatusHandler.HitType.Stun)
    {
        OnHit(damage, hitType);
        if (isIgnoreHitAnim || hitType == EnemyStatusHandler.HitType.Normal) return;
        
        statusHandler.SetMode(EnmeyMode.Hit, true);
        machine.Notify();
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void GetDamage(float damage)
    {
        // 타격 받은 쪽으로 회전
        // Vector2 direction = (controller.agent.player.transform.position - controller.transform.position).normalized;
        // controller.Flip(direction.x > 0);
        
        // 방어력 개념도 구현하기
        // statusHandler.stamina -= 1;
        // if (statusHandler.stamina <= 0) { statusHandler.stamina = 3; }
        
        OnHit(damage);
        statusHandler.SetMode(EnmeyMode.Hit, true);
        machine.Notify();
    }
    
    // 리워드 표시, 리스폰 아리어에서 제거
    // ReSharper disable Unity.PerformanceAnalysis
    public void Die()
    {
        gameObject.SetActive(false);
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.RemoveEnemy(this); }
        catch { gameObject.SetActive(false); }

        DamageTextManager.Instance.ShowExperience(rewardHandler.Experience);
        // 경험치 추가
        GameManager.Instance.player.playerstatus.GainExperience(rewardHandler.Experience);
        
        // 피봇 변경으로 인한 위치 조정
        if (rewardHandler)
        {
            SoundManager.Instance.Playsfx("DropItem");
            
            rewardHandler.DropCoin();
            rewardHandler.DropMemoryItem();
        }
    }

    private void OnDisable()
    {
        // Die 이후 초기화
        statusHandler.SetMode(EnmeyMode.Hit, false);
        Collider.enabled = true;
        Rigidbody.isKinematic = false;
    }
}
