using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : EnemyBaseController, IDamagable
{
    // SO로 추후 관리해도 좋을 듯
    [Header("Resource")]
    public float health;
    public float attack;


    public Enemies.Enemy name;
    
    public EnemyResourceHandler resourceHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }
    public EnemyRewardHandler rewardHandler { get; private set; }
    
    protected override void Awake()
    {
        base.Awake();
        
        resourceHandler = GetComponent<EnemyResourceHandler>();
        statusHandler = new EnemyStatusHandler();
        rewardHandler = GetComponent<EnemyRewardHandler>();
    }
    
    public void Start()
    {
        // Debug.Log(name.ToString());
        // 애니메이터 자동 등록
        animationHandler.SetController(EnemiesAnimator.animators["NightBone"]);
        // 에러처리 필요
        machine.Define(Enemies.Get(name)); // 각 개체별 생성되는 방식
        
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this); }
        catch { Debug.Log("there is no MapspawnManager"); }
    }


    public void GetDamage(float damage) // notice: summon은 제외
    {
        // resourceHandler에서 처리
        health -= damage;
        statusHandler.stamina -= 1;
        if (statusHandler.stamina <= 0) { statusHandler.stamina = 3; }

        statusHandler.isHit = true;
        machine.Notify();
    }

    // 리워드 표시, 리스폰 아리어에서 제거
    // ReSharper disable Unity.PerformanceAnalysis
    public void Die()
    {
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.RemoveEnemy(this); }
        catch { Destroy(gameObject); }
        
        if (rewardHandler) { Instantiate(rewardHandler.GetRewardItem(), transform.position, Quaternion.identity); }
    }
}
