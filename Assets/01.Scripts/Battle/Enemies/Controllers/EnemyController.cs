using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : EnemyBaseController, IDamagable
{
    // SO로 추후 관리해도 좋을 듯
    public float maxHealth { get; private set; }
    [Header("Resource")] 
    public float health;
    public float attack;
    
    public float experience;


    [FormerlySerializedAs("name")] public EnemiesBT.Enemy Name;
    
    public EnemyResourceHandler resourceHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }
    public EnemyRewardHandler rewardHandler { get; private set; }

    // children 시스템 등록
    // public List<EnemyController> children; 
    
    protected override void Awake()
    {
        base.Awake();
        
        resourceHandler = GetComponent<EnemyResourceHandler>();
        statusHandler = new EnemyStatusHandler();
        rewardHandler = GetComponent<EnemyRewardHandler>();
    }
    
    public void Start()
    {
        maxHealth = health; // 리소스 시스템 구현 필요
        // 애니메이터 자동 등록
        // animationHandler.SetController(EnemiesAnimator.animators["NightBone"]);
        // 에러처리 필요
        machine.Define(EnemiesBT.Get(Name)); // 각 개체별 생성되는 방식
        machine.Start();
        
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this); }
        catch { Debug.Log("there is no MapspawnManager"); }
    }


    public void GetDamage(float damage)
    {
        // resourceHandler에서 처리
        
        // 방어력 개념도 구현하기
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

        // 경험치 추가
        GameManager.Instance.player.playerstatus.GainExperience(experience);
        
        // 피봇 변경으로 인한 위치 조정
        if (rewardHandler)
        {
            SoundManager.Instance.Playsfx("DropItem");
            
            Instantiate(rewardHandler.GetRewardItem(), transform.position + (Vector3.up * 0.5f), Quaternion.identity);
            // 아이템이 없을 수도
            Instantiate(rewardHandler.money, transform.position + (Vector3.up * 0.5f), Quaternion.identity);
            
        }
    }
}
