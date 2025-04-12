using System;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyController : EnemyBaseController, IDamagable
{
    // SO로 추후 관리해도 좋을 듯
    [Header("Resource")]
    public float health;
    public float attack;
    public string monsterBehaviour; 
    
    public EnemyAgent agent { get; private set; }
    public EnemyResourceHandler resourceHandler { get; private set; }
    public EnemyStatusHandler statusHandler { get; private set; }
    public EnemyRewardHandler rewardHandler { get; private set; }
    
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        agent = GetComponent<EnemyAgent>();

        resourceHandler = GetComponent<EnemyResourceHandler>();
        statusHandler = new EnemyStatusHandler();
        rewardHandler = GetComponent<EnemyRewardHandler>();
    }
    
    public virtual void Start()
    {
        // 스킬 자체라면 스킬이름을 주입하면 됨
        btMachine.Define(Enemies.behaviours["goblin"]);
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.AddList(this); }
        catch { Debug.Log("there is no MapspawnManager"); }
    }


    public void GetDamage(float damage) // notice: summon은 제외
    {
        // Vector3 textPosition = transform.position + Vector3.up * 1f;
        // DamageTextManager.Instance.ShowDamage(textPosition, (int)damage);
        
        // resourceHandler에서 처리
        health -= damage;
        statusHandler.stamina -= 1;

        if (statusHandler.stamina <= 0)
        {
            statusHandler.stamina = 3;
            Debug.Log("knock out");
        }

        if (!statusHandler.isIgnoreHitAction)
        {
            statusHandler.isHit = true;
            btMachine.Notify();
        }

        if (health <= 0) Die();
    }

    // 리워드 표시, 리스폰 아리어에서 제거
    public void Die()
    {
        try { MapSpawnManager.Instance.SpawnedMap.monsterManager.RemoveEnemy(this); }
        catch { Destroy(gameObject); }
        
        if (rewardHandler) { Instantiate(rewardHandler.GetRewardItem(), transform.position, Quaternion.identity); }
    }
}
